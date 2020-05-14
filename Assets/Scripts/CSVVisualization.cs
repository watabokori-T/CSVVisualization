using UnityEngine;
using System.IO;

public class CSVVisualization : MonoBehaviour {

    public class csvdat
    {
        public double[] x;
        public double[] y;
        public double[] z;
        public double[] power;
    }

    [SerializeField]
    string bin_data;

    bool pressed;
    GameObject spheres, sphere;

    void Start()
    {
        pressed = false;
    }

    /// <summary>
    /// 球表示
    /// </summary>
    /// <param name="x">x座標</param>
    /// <param name="y">y座標</param>
    /// <param name="z">z座標</param>
    /// <param name="pow">直径</param>
    public void SetSphere(double x, double y, double z, double pow, int index)
    {
        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.name = "sphere" + index;
        sphere.transform.position = new Vector3((float)x , (float)y , (float)z);
        sphere.transform.localScale = new Vector3((float)pow / 100, (float)pow / 100, (float)pow / 100);
        sphere.transform.parent = spheres.transform;
        sphere.GetComponent<Renderer>().material.color = new Color(1f-(float)pow, (float)1f, 1f-(float)pow, 0.5f);
    }

    /// <summary>
    /// ファイル読込
    /// </summary>
    /// <param name="bin_data">データ名</param>
    public void BinRead(string bin_data)
    {
        csvdat dat = new csvdat();
        TextAsset asset = Resources.Load(bin_data) as TextAsset;

        using (Stream fs = new MemoryStream(asset.bytes))
        {
            using (BinaryReader br = new BinaryReader(fs))
            {
                var baseStream = br.BaseStream;
                int array_num = (int)baseStream.Length / 4 / sizeof(double);
                dat.x = new double[array_num];
                dat.y = new double[array_num];
                dat.z = new double[array_num];
                dat.power = new double[array_num];

                int i = 0;
                while(baseStream.Position != baseStream.Length)
                {
                    dat.x[i] = br.ReadDouble();
                    dat.y[i] = br.ReadDouble();
                    dat.z[i] = br.ReadDouble();
                    dat.power[i] = br.ReadDouble();

                    SetSphere(dat.x[i], dat.y[i], dat.z[i], dat.power[i], i);
                    i++;
                }
            }
        }
    }

    /// <summary>
    /// セット
    /// </summary>
    /// <param name="bin_data">ファイル名</param>
    public void SetData(string bin_data)
    {
        spheres = new GameObject("Spheres");
        BinRead(bin_data);
        spheres.transform.rotation = Quaternion.Euler(-90, 0, 180);
    }

    /// <summary>
    /// 消去
    /// </summary>
    public void SpheresClear()
    {
        GameObject data = GameObject.Find("/Spheres");
        Destroy(data);
    }

    /// <summary>
    /// UI操作
    /// </summary>
    public void OnClick()
    {
        if (pressed == false)
        {
            SetData(bin_data);
            pressed = true;
        }
        else if (pressed == true)
        {
            SpheresClear();
            pressed = false;
        }    
    }

    /// <summary>
    /// PC操作
    /// </summary>
    void Update()
    {
        if (Input.GetKey(KeyCode.T))
        {
            if (pressed == false)
            {
                SetData(bin_data);
                pressed = true;
            }
            else if (pressed == true)
            {
                SpheresClear();
                pressed = false;
            }          
        }
    }
}
