using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Classes : MonoBehaviour
{
    [SerializeField] private GameObject classeObj;
    [SerializeField] private GameObject racaObj;
    [SerializeField] private GameObject nivelObj;
    [SerializeField] private GameObject[] subclasses;
    [SerializeField] private GameObject[] subracas;
    [SerializeField] private GameObject objEstiloDeLutaG;
    [SerializeField] private GameObject objEstiloDeLutaP;
    [SerializeField] private GameObject objEstiloDeLutaPaladinos;
    [SerializeField] private GameObject objPactoBruxo;
    [SerializeField] private GameObject objCirculoDaTerra;
    [SerializeField] private GameObject textoSubRaca;
    [SerializeField] private GameObject textoSubClasse;
    [SerializeField] private GameObject textoEstiloLuta;
    [SerializeField] private GameObject textoPacto;
    [SerializeField] private GameObject textoOrigemDruida;
    private int qualCirculo = 0;
    private int estiloDeLutaPaladino = 0;
    private int estiloDeLutaG = 0;
    private int estiloDeLutaP = 0;
    private int pacto = 0;
    private List<string> manobrasGUERREIRO = new List<string>() { };
    private int qualSubClasse = -1;
    private string classe;
    private GameObject labelRaca, labelClasse;
    private Controller com;
    private string especificidade = "";

    private static readonly Dictionary<string, int> objSubClasse = new()
    {
        //AQUI
        { "Barbaro", 0 },
        { "Bardo", 1},
        { "Bruxo", 2 },
        { "Clérigo", 3 },
        { "Druida", 4 },
        { "Feiticeiro", 5},
        { "Guerreiro", 6 },
        { "Ladino", 7 },
        { "Mago", 8 },
        { "Monge", 9 },
        { "Paladino", 10 },
        { "Patrulheiro", 11 },

    };
    private static readonly Dictionary<string, int> objSubRaca = new()
    {
        { "Escolher Raça", -1 },
        { "Anão", 0 },
        { "Elfo", 1 },
        { "Halfling", 2 },
        { "Humano", -1 },
        { "Draconato", 3 },
        { "Gnomo", 4 },
        { "Meio-Elfo", -1 },
        { "Meio-Orc", -1 },
        { "Tiefling", -1 }

    };

    void Start()
    {
        com = FindObjectOfType<Controller>();
    }

    void Update()
    {
        
    }

    public void ResetaTudo()
    {
        manobrasGUERREIRO = new List<string>();
        especificidade = "";
    }

    //AQUI
    private void AtivaEspecificos(int parametro)
    {
        switch (parametro)
        {
            case 2:
                objPactoBruxo.SetActive(true);
                textoPacto.SetActive(true);
                break;
            case 6:
                objEstiloDeLutaG.SetActive(true);
                textoEstiloLuta.SetActive(true);
                break;
            case 10:
                objEstiloDeLutaPaladinos.SetActive(true);
                textoEstiloLuta.SetActive(true);
                break;
            case 11:
                objEstiloDeLutaP.SetActive(true);
                textoEstiloLuta.SetActive(true);
                break;
        }
    }

    public void PegaEspecificidade(string esp)
    {
        especificidade = esp;
    }

    public string PassaEspecificidade()
    {
        return especificidade;
    }

    public void MudaEstiloDeLutaG()
    {
        estiloDeLutaG = objEstiloDeLutaG.GetComponent<TMP_Dropdown>().value;
    }

    public void MudaEstiloDeLutaP()
    {
        estiloDeLutaP = objEstiloDeLutaP.GetComponent<TMP_Dropdown>().value;
    }

    public void MudaEstiloDeLutaPaladino()
    {
        estiloDeLutaPaladino = objEstiloDeLutaPaladinos.GetComponent<TMP_Dropdown>().value;
    }

    public void MudaPactoBruxo()
    {
        pacto = objPactoBruxo.GetComponent<TMP_Dropdown>().value;
    }

    private void DesativaObjsSubRaca()
    {
        for(int i = 0; i < 5; i++)
        {
            subracas[i].GetComponent<TMP_Dropdown>().value = 0;
            subracas[i].SetActive(false);
        }
        textoSubRaca.SetActive(false);
    }

    //AQUI
    public void AplicaClasse()
    {
        switch (classe)
        {
            case "Barbaro":
                Barbaro bar = FindObjectOfType<Barbaro>();
                bar.LendoArquivo();
                break;
            case "Bruxo":
                Bruxo bru = FindObjectOfType<Bruxo>();
                bru.LendoArquivo(pacto-1);
                break;
            case "Bardo":
                Bardo bard = FindObjectOfType<Bardo>();
                bard.LendoArquivo();
                break;
            case "Clérigo":
                Clerigo cle = FindObjectOfType<Clerigo>();
                cle.LendoArquivo();
                break;
            case "Druida":
                Druida dru = FindObjectOfType<Druida>();
                dru.LendoArquivo(qualCirculo-1);
                break;
            case "Feiticeiro":
                Feiticeiro fei = FindObjectOfType<Feiticeiro>();
                fei.LendoArquivo();
                break;
            case "Guerreiro":
                Guerreiro gue = FindObjectOfType<Guerreiro>();
                gue.LendoArquivo(estiloDeLutaG);
                break;
            case "Ladino":
                Ladino lad = FindObjectOfType<Ladino>();
                lad.LendoArquivo();
                break;
            case "Mago":
                Mago mag = FindObjectOfType<Mago>();
                mag.LendoArquivo();
                break;
            case "Monge":
                Monge mon = FindObjectOfType<Monge>();
                mon.LendoArquivo();
                break;
            case "Paladino":
                Paladino pal = FindObjectOfType<Paladino>();
                pal.LendoArquivo(estiloDeLutaPaladino - 1);
                break;
            case "Patrulheiro":
                Patrulheiro pat = FindObjectOfType<Patrulheiro>();
                pat.LendoArquivo(estiloDeLutaP);
                break;
            default:
                Debug.LogError("ERRO!!!");
                break;
        }
    }

    public void EscolheNivel()
    {
        int niv = nivelObj.GetComponent<TMP_Dropdown>().value;
        com.RecebeNivel(niv+1);
    }

    public bool VerificaManobrasJaEscolhida(string manobra)
    {
        foreach(string man in manobrasGUERREIRO)
        {
            if(man == manobra)
            {
                return true;
            }
        }
        manobrasGUERREIRO.Add(manobra);
        return false;
    }
    
    public List<string> PassaManobrasGuerreiro()
    {
        return manobrasGUERREIRO;
    }

    public void EscolheClasse()
    {
        //Desativando itens na tela
        for (int i = 0; i < subclasses.Length; i++)
        {
            if(subclasses[i] != null)
            {
                subclasses[i].SetActive(false);
                subclasses[i].GetComponent<TMP_Dropdown>().value = 0;
            }
        }
        textoEstiloLuta.SetActive(false);
        textoPacto.SetActive(false);
        textoOrigemDruida.SetActive(false);
        objEstiloDeLutaG.SetActive(false);
        objEstiloDeLutaG.GetComponent<TMP_Dropdown>().value = 0;
        objEstiloDeLutaP.SetActive(false);
        objEstiloDeLutaP.GetComponent<TMP_Dropdown>().value = 0;
        objPactoBruxo.SetActive(false);
        objPactoBruxo.GetComponent<TMP_Dropdown>().value = 0;
        objCirculoDaTerra.SetActive(false);
        objCirculoDaTerra.GetComponent<TMP_Dropdown>().value = 0;
        objEstiloDeLutaPaladinos.SetActive(false);
        objEstiloDeLutaPaladinos.GetComponent<TMP_Dropdown>().value = 0;

        string clas = classeObj.GetComponent<TextMeshProUGUI>().text;
        classe = clas;
        com.RecebeClasse(clas);
        if(clas != "Escolher Classe")
        {
            int parametro = objSubClasse[clas];
            subclasses[parametro].SetActive(true);
            textoSubClasse.SetActive(true);

            labelClasse = subclasses[parametro];
            AtivaEspecificos(parametro);
        }
        else
        {
            textoSubClasse.SetActive(false);
        }

    }

    public void EscolheRaca()
    {
        string raca = racaObj.GetComponent<TextMeshProUGUI>().text;
        com.RecebeRaca(raca);
        int parametro = objSubRaca[raca];
        DesativaObjsSubRaca();
        if(parametro != -1)
        {
            textoSubRaca.SetActive(true);
            subracas[parametro].SetActive(true);
            labelRaca = subracas[parametro];
        }
    }

    public string PassaRaca()
    {
        string raca = racaObj.GetComponent<TextMeshProUGUI>().text;
        return raca;
    }

    public int PassaSubRaca()
    {
        if(labelRaca != null)
        {
            int subR = labelRaca.GetComponent<TMP_Dropdown>().value;
            return (subR - 1);
        }
        else
        {
            return -1;
        }
        
    }

    public void EscolheSubRaca()
    {

        int subR = labelRaca.GetComponent<TMP_Dropdown>().value;
        com.RecebeSubRaca(subR - 1);
    }

    public void CirculodaTerra()
    {
        qualCirculo = objCirculoDaTerra.GetComponent<TMP_Dropdown>().value;
    }

    public void EscolheSubClasse()
    {
        objCirculoDaTerra.SetActive(false);
        textoOrigemDruida.SetActive(false);
        objCirculoDaTerra.GetComponent<TMP_Dropdown>().value = 0;
        var teste = labelClasse.GetComponent<TMP_Dropdown>();
        int subC = teste.value;
        qualSubClasse = subC - 1;
        if(classe == "Druida" && subC == 1)
        {
            objCirculoDaTerra.SetActive(true);
            textoOrigemDruida.SetActive(true);
        }
    }

    public int PassaSubClasse()
    {
        return qualSubClasse;
    }

}
