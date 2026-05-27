using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using SFB;


public class Controller : MonoBehaviour
{
    [SerializeField] private Text forca, destreza, constituicao, inteligencia, sabedoria, carisma;
    [SerializeField] private Text modForca, modDestreza, modConstituicao, modInteligencia, modSabedoria, modCarisma;
    private string classe = "VAZIO";
    private string raca = "Escolher Raça";
    [SerializeField] private Text idiomasTexto;
    [SerializeField] private Text truquesTexto;
    [SerializeField] private Text habilidadesTexto;
    [SerializeField] private Text proficienciaTexto;
    [SerializeField] private Text iniciativaTexto;
    [SerializeField] private Text classeAmaduraTexto;
    [SerializeField] private Text classeENivel;
    [SerializeField] private Text deslocamentoTexto;
    [SerializeField] private Text vidaTexto;
    [SerializeField] private Text racaTexto;
    [SerializeField] private GameObject[] iconeSalvaGuardas;
    [SerializeField] private Transform[] posFichas;
    [SerializeField] private GameObject[] canvas;
    [SerializeField] private Text textoHabilidadesClasse;
    [SerializeField] private Text textoHabilidadesClasse2;
    [SerializeField] private Text textoHabilidadesClasse3;
    [SerializeField] private Text textoMagiasClasse;
    [SerializeField] private Text nome;
    [SerializeField] private Text textoNome;
    [SerializeField] private GameObject generoNome;
    [SerializeField] private GameObject botaoSalvar;
    [SerializeField] private Classes cla;
    private int modMagia = 0;
    private int cdMagia = 0;
    private Text[] textos = { null, null, null };
    public bool manobra;
    private int classeArmadura = 10;
    private int qualTextoUsar = 0;
    private string armadura = "";
    private int[] quantidadeEspacosMagia = { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    private List<string>[] magias = { new List<string>() { }, new List<string>() { }, new List<string>() { }, new List<string>() { }, new List<string>() { }, new List<string>() { }, new List<string>() { }, new List<string>() { }, new List<string>() { } };
    private int nivel = 1;
    [SerializeField] private GameObject erroClasse;
    private bool[] pericias = { false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false, false };
    [SerializeField]  private GameObject[] iconePericias;
    private int deslocamento;
    private int qualSubRaca = -1;
    private List<string> habilidades = new List<string>() { };
    private List<string> habilidadesClasse = new List<string>() { };
    private List<string> truques = new List<string>() { };
    private List<string> idiomas = new List<string>() {};
    private int[] preferencias = { 0, 1 };
    private int[] aTRIBUTOS = { 15, 14, 13, 12, 10, 8 };
    private int[] atributos = { 0, 0, 0, 0, 0, 0 };

    [SerializeField] private Camera cam1, cam2, cam3;
    [SerializeField] private int[] rect;


    private static readonly Dictionary<string, int[]> classesPrioridade = new()
    {
        { "Barbaro", new int[] { 0, 2 } },
        { "Bardo", new int[] { 5, 1 } },
        { "Bruxo", new int[] { 5, 2 } },
        { "Clérigo", new int[] { 4, 0 } },
        { "Druida", new int[] { 4, 2 } },
        { "Feiticeiro", new int[] { 5, 2 } },
        { "Guerreiro", new int[] { 0, 2 } },
        { "Ladino", new int[] { 1, 3 } },
        { "Mago", new int[] { 3, 2 } },
        { "Monge", new int[] { 1, 4 } },
        { "Paladino", new int[] { 0, 5 } },
        { "Patrulheiro", new int[] { 1, 4 } }
    };
    private static readonly Dictionary<string, int[]> classesResistencia = new()
    {
        { "Barbaro", new int[] { 0, 2 } },
        { "Bardo", new int[] { 5, 1 } },
        { "Bruxo", new int[] { 5, 4 } },
        { "Clérigo", new int[] { 4, 5 } },
        { "Druida", new int[] { 4, 3 } },
        { "Feiticeiro", new int[] { 5, 2 } },
        { "Guerreiro", new int[] { 0, 2 } },
        { "Ladino", new int[] { 1, 3 } },
        { "Mago", new int[] { 3, 4 } },
        { "Monge", new int[] { 1, 0 } },
        { "Paladino", new int[] { 4, 5 } },
        { "Patrulheiro", new int[] { 1, 0 } }
    };

    private static readonly Dictionary<string, int> classesDadoVida = new()
    {
        { "Barbaro", 12 },
        { "Bardo", 8 },
        { "Bruxo", 8 },
        { "Clérigo", 8 },
        { "Druida", 8 },
        { "Feiticeiro", 6 },
        { "Guerreiro", 10 },
        { "Ladino", 8 },
        { "Mago", 6 },
        { "Monge", 8 },
        { "Paladino", 10 },
        { "Patrulheiro", 10 }
    };

    private static readonly Dictionary<string, string> arquivosRacas = new()
    {
        { "Anão", "Texts\\Raças\\Anão.txt" },
        { "Draconato", "Texts\\Raças\\Draconato.txt" },
        { "Elfo", "Texts\\Raças\\Elfo.txt" },
        { "Gnomo", "Texts\\Raças\\Gnomo.txt" },
        { "Halfling", "Texts\\Raças\\Halfling.txt" },
        { "Humano", "Texts\\Raças\\Humano.txt" },
        { "Meio-Elfo", "Texts\\Raças\\Meio-Elfo.txt" },
        { "Meio-Orc", "Texts\\Raças\\Meio-Orc.txt" },
        { "Tiefling", "Texts\\Raças\\Tiefling.txt" }
    };

    private static readonly Dictionary<string, int> quantasSubRacas = new()
    {
        { "Anão", 2 },
        { "Draconato", 1 },
        { "Elfo", 3 },
        { "Gnomo", 2 },
        { "Halfling", 2 },
        { "Humano", 1 },
        { "Meio-Elfo", 1 },
        { "Meio-Orc", 1 },
        { "Tiefling", 1 }
    };

    private static readonly Dictionary<int, string> racaAleatoria = new()
    {
        { 0,"Anão"},
        { 1,"Draconato"},
        { 2,"Elfo"},
        { 3,"Gnomo"},
        { 4,"Halfling"},
        { 5,"Humano"},
        { 6,"Meio-Elfo"},
        { 7,"Meio-Orc"},
        { 8,"Tiefling"}
    };

    private static readonly Dictionary<int, string> idiomasDnD = new()
    {
        { 0, "Élfico" },
        { 1, "Orc" },
        { 2, "Anão" },
        { 3, "Gigante" },
        { 4, "Gnômico" },
        { 5, "Goblin" },
        { 6, "Halfling" },
        { 7, "Dracônico" },
        { 8, "Sub-Comum" }
    };
 
    private static readonly Dictionary<string, int> periciasPos = new()
    {
        { "Acrobacia", 0 },
        { "Arcanismo", 1 },
        { "Atletismo", 2 },
        { "Atuação", 3 },
        { "Enganação", 4 },
        { "Furtividade", 5 },
        { "História", 6 },
        { "Intimidação", 7 },
        { "Intuição", 8 },
        { "Investigação", 9 },
        { "Lidar com Animais", 10 },
        { "Medicina", 11 },
        { "Natureza", 12 },
        { "Percepção", 13 },
        { "Persuasão", 14 },
        { "Prestidigitação", 15 },
        { "Religião", 16 },
        { "Sobrevivência", 17 }

    };



    void Start()
    {
        textos[0] = textoHabilidadesClasse;
        textos[1] = textoHabilidadesClasse2;
        textos[2] = textoHabilidadesClasse3;
    }



    public void SalvaPdf()
    {
        RenderTexture rem1 = cam1.targetTexture, rem2 = cam2.targetTexture, rem3 = cam3.targetTexture;
        Rect esp = new Rect(0, 0, rect[0], rect[1]);
        Texture2D tex = new Texture2D((int)esp.width, (int)esp.height, TextureFormat.RGBA32, false);

        byte[][] pngData = new byte[3][];

        RenderTexture.active = rem1;
        tex.ReadPixels(esp, 0, 0);
        tex.Apply();
        pngData[0] = tex.EncodeToPNG();

        RenderTexture.active = rem2;
        tex.ReadPixels(esp, 0, 0);
        tex.Apply();
        pngData[1] = tex.EncodeToPNG();

        RenderTexture.active = rem3;
        tex.ReadPixels(esp, 0, 0);
        tex.Apply();
        pngData[2] = tex.EncodeToPNG();

        RenderTexture.active = null;

        // Cria o PDF
        PdfDocument document = new PdfDocument();

        foreach (byte[] imageBytes in pngData)
        {
            PdfPage page = document.AddPage();

            using (XGraphics gfx = XGraphics.FromPdfPage(page))
            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                XImage img = XImage.FromStream(() => ms);

                // Ajusta página ao tamanho da imagem
                page.Width = img.PixelWidth;
                page.Height = img.PixelHeight;

                gfx.DrawImage(img, 0, 0, page.Width, page.Height);
            }
        }

        var path = StandaloneFileBrowser.SaveFilePanel(
        "Salvar PDF",
        "",
        "arquivo",
        "pdf"
        );

        if (!string.IsNullOrEmpty(path))
        {
            document.Save(path);
        }
 
    }

    public void AplicaCDMagias(int qualAtributo)
    {
        int modAtributo = (int)(atributos[qualAtributo] / 2 - 5);
        cdMagia = 8 + modAtributo + int.Parse(proficienciaTexto.text);
        modMagia = int.Parse(proficienciaTexto.text) + modAtributo;
    }

    private void AplicaMagias()
    {
        string espec = cla.PassaEspecificidade();
        if (espec != "")
        {
            textoMagiasClasse.text += $"{espec}\n\n";
        }
        if(truques.Count > 0 || quantidadeEspacosMagia.Length > 0)
        {
            textoMagiasClasse.text += "<<Magias>>\n";
            textoMagiasClasse.text += $"CD: {cdMagia}    MOD: {modMagia}\n\n";
            textoMagiasClasse.text += "Espaços de Magia:\n<1>|<2>|<3>|<4>|<5>|<6>|<7>|<8>|<9>\n ";
            foreach(int espacos in quantidadeEspacosMagia)
            {
                textoMagiasClasse.text += $"({espacos})   ";
            }
            textoMagiasClasse.text += "\n\n";
            foreach (List<string> listasMagia in magias)
            {
                foreach(string magia in listasMagia)
                {
                    textoMagiasClasse.text += $"{magia}\n";
                }
            }
            textoMagiasClasse.text += "\nTruques:\n\n";
            foreach(string truque in truques)
            {
                textoMagiasClasse.text += $"{truque}\n";
            }
            textoMagiasClasse.text += "\n";
        }
        
    }
    
    public void PegaClasseArmadura(int classeBase, bool destreza, bool destrezaLimitada)
    {
        int final = classeBase;
        int modificador = (int)(atributos[1] / 2) - 5;
        if (destreza)
        {
            if (destrezaLimitada)
            {
                if(modificador > 2)
                {
                    final += 2;
                }
                else
                {
                    final += modificador;
                }
            }
            else
            {
                final += modificador;
            }
        }
        if(classeBase == 10)
        {
            final = 10 + modificador;
        }
        classeArmadura = final;
    }

    public int PassaClasseArmadura()
    {
        return classeArmadura;
    }

    public int PassaModificadorAtributo(int qual)
    {
        int mod = (int)(atributos[qual] / 2 - 5);
        return mod;
    }

    public void QualArmadura(string armadura_)
    {
        armadura = armadura_;
    }

    public void ColocaEspacosMagia(int quantos, int qual)
    {
        quantidadeEspacosMagia[qual - 1] = quantos;
    }

    public void AdicionaHabilidadeClasse(string hab)
    {
        habilidadesClasse.Add(hab);
    }

    public void CanalizarDivindade(List<string> habilidades)
    {
        textoHabilidadesClasse3.text += "Canalizar Divindade:\n\n";
        foreach(string habilidade in habilidades)
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Texts/Habilidades/Clérigo.txt");
            string[] listaHabilidadesClasse = File.ReadAllLines(path, System.Text.Encoding.UTF7);
            for (int i = 0; i < listaHabilidadesClasse.Length; i += 2)
            {
                if (habilidade == listaHabilidadesClasse[i])
                {
                    //Aplicando texto
                    textoHabilidadesClasse3.text += habilidade;
                    textoHabilidadesClasse3.text += ":\n";
                    textoHabilidadesClasse3.text += listaHabilidadesClasse[i + 1];
                    textoHabilidadesClasse3.text += "\n\n";
                    break;
                }
            }
        }
        
    }

    public void CanalizarDivindadePaladino(List<string> habilidades)
    {
        textoHabilidadesClasse3.text += "Canalizar Divindade:\n\n";
        foreach (string habilidade in habilidades)
        {
            string path = Path.Combine(Application.streamingAssetsPath, "Texts/Habilidades/Paladino.txt");
            string[] listaHabilidadesClasse = File.ReadAllLines(path, System.Text.Encoding.UTF7);
            for (int i = 0; i < listaHabilidadesClasse.Length; i += 2)
            {
                if (habilidade == listaHabilidadesClasse[i])
                {
                    //Aplicando texto
                    textoHabilidadesClasse3.text += habilidade;
                    textoHabilidadesClasse3.text += ":\n";
                    textoHabilidadesClasse3.text += listaHabilidadesClasse[i + 1];
                    textoHabilidadesClasse3.text += "\n\n";
                    break;
                }
            }
        }

    }

    public bool TentaAplicarMagia(string mag, int nivelMag)
    {
        for(int i = 0; i < magias[nivelMag-1].Count; i++)
        {
            if(mag == magias[nivelMag - 1][i])
            {
                return true;
            }
        }
        magias[nivelMag - 1].Add(mag);
        return false;
    }

    public int TentaAplicarPericia(string pericia)
    {
        int pos = periciasPos[pericia];
        if (pericias[pos])
        {
            return 0;
        }
        else
        {
            pericias[pos] = true;
            return 1;
        }
    }

    public bool EstaNosTruques(string truque)
    {
        foreach(string tru in truques)
        {
            if(truque == tru)
            {
                return true;
            }
        }
        truques.Add(truque);
        return false;
    }

    public int PassaNivel()
    {
        return nivel;
    }

    public void RecebeNivel(int niv)
    {
        nivel = niv;
    }

    public void RecebeClasse(string clas)
    {
        classe = clas;
    }

    public void RecebeRaca(string rac)
    {
        raca = rac;
    }

    public void RecebeSubRaca(int sub)
    {
        qualSubRaca = sub;
    }

    private void ErroClasse()
    {
        erroClasse.SetActive(true);
        Invoke("DesativaErroClasse", 3f);
    }

    private void DesativaErroClasse()
    {
        erroClasse.SetActive(false);
    }

    public void GeraFicha()
    {
        //Resetando
        //-----------------------------------
        //-----------------------------------
        if(cla.PassaRaca() == "Escolher Raça")
        {
            raca = "Escolher Raça";
        }
        if(cla.PassaSubRaca() == -1)
        {
            qualSubRaca = -1;
        }
        textos[0].text = "HABILIDADES:\n\n";
        textos[1].text = "";
        textos[2].text = "";
        qualTextoUsar = 0;
        textoNome.text = "";
        textoMagiasClasse.text = "";
        ResetaAtributos();
        idiomasTexto.text = "";
        manobra = false;
        habilidades = new List<string>() { };
        habilidadesClasse = new List<string>() { };
        truques = new List<string>() { };
        idiomas = new List<string>() { };
        int[] aTRIBUTOSn = { 15, 14, 13, 12, 10, 8 };
        aTRIBUTOS = aTRIBUTOSn;
        for (int i = 0; i < atributos.Length; i++)
        {
            atributos[i] = 0;
        }
        for(int i = 0; i < pericias.Length; i++)
        {
            pericias[i] = false;
        }
        for(int i = 0; i < iconeSalvaGuardas.Length; i++)
        {
            iconeSalvaGuardas[i].SetActive(false);
        }
        for(int i = 0; i<quantidadeEspacosMagia.Length; i++)
        {
            quantidadeEspacosMagia[i] = 0;
            magias[i] = new List<string>();
        }
        for(int i = 0; i < iconePericias.Length; i++)
        {
            iconePericias[i].SetActive(false);
        }
        cla.ResetaTudo();
        //-----------------------------------
        //-----------------------------------
        if (classe == "VAZIO" || classe == "Escolher Classe")
        {
            ErroClasse();
            return;
        }
        preferencias = classesPrioridade[classe];
        EscolheAtributoPrincipal(preferencias[0]);
        EscolheAtributoSecundario(preferencias[1]);
        EscolherAtributosFaltando(preferencias);
        AumentaAtributos(preferencias);
        AplicaRaca();
        ColocaAtributosNaFicha();
        ColocaModificadoresNaFicha();
        AplicaTruques();
        AplicaResistencia();
        AplicaBonusProficiencia();
        AplicaVida();
        cla.AplicaClasse();
        AplicaAntecedentesAleatorio();
        AplicaIdiomas();
        classeAmaduraTexto.text = classeArmadura.ToString();
        AplicaPericias();
        AplicaHabidadesClasse();
        AplicaHabilidades();
        AplicaMagias();

        botaoSalvar.SetActive(true);
    }

    private void AplicaAntecedentesAleatorio()
    {
        //Colocando o idioma
        int rand, contador = 0;
        bool jaTem;
        do
        {
            jaTem = false;
            rand = Random.Range(0, idiomasDnD.Count);
            for (int i = 0; i < idiomas.Count; i++)
            {
                if (idiomas[i] == idiomasDnD[rand])
                {
                    jaTem = true;
                }
            }
            if (contador > 20)
            {
                break;
            }
            contador++;
        } while (jaTem);
        if(contador <= 20) { idiomas.Add(idiomasDnD[rand]); }

        //Colocando pericias
        int randP, contadorP;
        for(int i = 0; i < 2; i++)
        {
            contadorP = 0;
            do
            {
                randP = Random.Range(0, pericias.Length);
                if (!pericias[randP]) 
                {
                    pericias[rand] = true;
                    break;
                }
                if(contadorP > 20)
                {
                    break;
                }
                contadorP++;
            }
            while (true);
        }

    }

    public void AdicionaHabilidadeDireto(string hab)
    {
        textoHabilidadesClasse.text += $"{hab}\n\n";
    }

    private void AplicaHabidadesClasse()
    {
        foreach(string habilidade in habilidadesClasse)
        {
            string path = Path.Combine(Application.streamingAssetsPath, $"Texts/Habilidades/{classe}.txt");
            string[] listaHabilidadesClasse = File.ReadAllLines(path, System.Text.Encoding.UTF7);
            for(int i = 0; i < listaHabilidadesClasse.Length; i+= 2)
            {
                if (habilidade == listaHabilidadesClasse[i])
                {
                    //Guardando texto para poder restaurar se preciso
                    string guardaTexto = textos[qualTextoUsar].text;
                    //Aplicando texto
                    textos[qualTextoUsar].text += habilidade;
                    textos[qualTextoUsar].text += ":\n";
                    textos[qualTextoUsar].text += listaHabilidadesClasse[i+1];
                    textos[qualTextoUsar].text += "\n\n";
                    //Preparando para testar se o texto passou os limites
                    textos[qualTextoUsar].cachedTextGenerator.Invalidate();
                    textos[qualTextoUsar].cachedTextGeneratorForLayout.Invalidate();
                    float alturaNecessaria = textos[qualTextoUsar].preferredHeight;
                    float alturaCaixa = ((RectTransform)textos[qualTextoUsar].transform).rect.height;
                    if(alturaNecessaria > alturaCaixa)
                    {
                        //Restaurando o texto que estava antes
                        textos[qualTextoUsar].text = guardaTexto;
                        //Passando para colocar no outro texto
                        qualTextoUsar++;
                        //Colocando finalmente o texto
                        textos[qualTextoUsar].text += habilidade;
                        textos[qualTextoUsar].text += ":\n";
                        textos[qualTextoUsar].text += listaHabilidadesClasse[i + 1];
                        textos[qualTextoUsar].text += "\n\n";
                    }
                    break;
                }
            }
        }
        if (manobra)
        {
            Classes cla = FindObjectOfType<Classes>();
            List<string> manobras = cla.PassaManobrasGuerreiro();
            string path = Path.Combine(Application.streamingAssetsPath, "Texts/Habilidades/Guerreiro.txt");
            string[] habil = File.ReadAllLines(path, System.Text.Encoding.UTF7);
            textos[2].text += "Manobras\n\n";
            foreach (string man in manobras)
            {
                textos[2].text += $"{man}:\n";
                for (int i = 0; i < habil.Length; i += 2)
                {
                    if (habil[i] == man)
                    {
                        textos[2].text += $"{habil[i + 1]}\n\n";
                    }
                }
            }
        }
    }

    private void AumentaAtributos(int[] preferencias)
    {
        int subirQuantos = nivel / 4;
        if(nivel == 20) { subirQuantos--; }
        for(int i = 0; i<subirQuantos; i++)
        {
            int rand = Random.Range(1, 16);
            if (rand <= 7)
            {
                atributos[preferencias[0]]++;
            }
            else if(rand <= 11)
            {
                atributos[preferencias[1]]++;
            }
            else
            {
                int[] parametros = { 0, 1, 2, 3, 4, 5 };
                parametros = parametros.Where(n => n != preferencias[0]).ToArray();
                parametros = parametros.Where(n => n != preferencias[1]).ToArray();
                int rando = Random.Range(0, 4);
                atributos[rando]++;
            }
        }
    }

    private void AplicaVida()
    {
        int dado = classesDadoVida[classe];
        int vida = dado + int.Parse(modConstituicao.text);
        for(int i = 0; i < nivel-1; i++)
        {
            int n = Random.Range(1, dado + 1);
            vida += (n + int.Parse(modConstituicao.text));
        }
        if(raca == "Anão" && qualSubRaca == 0)
        {
            vida += nivel;
        }
        vidaTexto.text = vida.ToString();

        string texto = "";
        texto += classe;
        texto += " ";
        texto += nivel.ToString();
        classeENivel.text = texto;

        racaTexto.text = raca;
    }

    public void AdicionaVida(int quanto)
    {
        int vida = int.Parse(vidaTexto.text);
        vida += quanto;
        vidaTexto.text = vida.ToString();
    }

    private void AplicaBonusProficiencia()
    {
        int bonusProficiencia = 2;
        if(nivel%4f == 0f)
        {
            bonusProficiencia = (nivel / 4) + 1;
        }
        else
        {
            bonusProficiencia = (nivel / 4) + 2;
        }
        proficienciaTexto.text = bonusProficiencia.ToString();
    }

    public bool VerificaHabiidadeRaca(string hab)
    {
        foreach(string habi in habilidades)
        {
            if(habi == hab)
            {
                return true;
            }
        }
        habilidades.Add(hab);
        return false;
    }

    private void AplicaHabilidades()
    {
        string texto = "";
        string path = Path.Combine(Application.streamingAssetsPath, "Texts/Raças/Habilidades.txt");
        string[] arquivoHabilidades = File.ReadAllLines(path, System.Text.Encoding.UTF7);
        for(int i = 0; i < habilidades.Count; i++)
        {
            for(int j = 0; j < arquivoHabilidades.Length; j += 2)
            {
                if(habilidades[i] == arquivoHabilidades[j])
                {
                    texto += habilidades[i];
                    texto += ":\n";
                    texto += arquivoHabilidades[j + 1];
                    if(i != habilidades.Count - 1)
                    {
                        texto += '\n';
                    }
                    break;
                }
            }
        }
        habilidadesTexto.text = texto;
        
    }

    private void AplicaResistencia()
    {
        int[] salvas = classesResistencia[classe];
        iconeSalvaGuardas[salvas[0]].SetActive(true);
        iconeSalvaGuardas[salvas[1]].SetActive(true);
    }

    public void AdicionaDeslocamento(int quantosQuadrados)
    {
        int des = int.Parse(deslocamentoTexto.text);
        des += quantosQuadrados;
        deslocamentoTexto.text = des.ToString();
    }
    
    private void AplicaTruques()
    {
        string texto = "";
        for(int i = 0; i < truques.Count; i++)
        {
            texto += truques[i];
            if(i != truques.Count - 1)
            {
                texto += ", ";
            }
        }
        texto += ".";
        truquesTexto.text = texto;
    }

    private void AplicaIdiomas()
    {
        string texto = "Comum";
        for(int i = 0; i < idiomas.Count; i++)
        {
            texto += ", ";
            texto += idiomas[i];
        }
        texto += ".";
        idiomasTexto.text = texto;
    }

    private void AplicaPericias()
    {
        for(int i = 0; i < 18; i++)
        {
            if (pericias[i])
            {
                iconePericias[i].SetActive(true);
            }
        }
    }

    private void AplicaRaca()
    {
        if(raca == "Escolher Raça")
        {
            int rand = Random.Range(0, 9);
            raca = racaAleatoria[rand];
        }

        if(nome.text == "")
        {
            string path2 = Path.Combine(Application.streamingAssetsPath, "Texts/Raças/Nomes.txt");
            string[] arquivoNomes = File.ReadAllLines(path2, System.Text.Encoding.UTF7);
            string possiveisNomes = "";
            if (raca == "Anão")
            {
                if(generoNome.GetComponent<Slider>().value == 1)
                {
                    possiveisNomes = arquivoNomes[0];
                }
                else
                {
                    possiveisNomes = arquivoNomes[1];
                }  
            }
            else if(raca == "Draconato")
            {
                if (generoNome.GetComponent<Slider>().value == 1)
                {
                    possiveisNomes = arquivoNomes[2];
                }
                else
                {
                    possiveisNomes = arquivoNomes[3];
                }
            }
            else if(raca == "Elfo")
            {
                if (generoNome.GetComponent<Slider>().value == 1)
                {
                    possiveisNomes = arquivoNomes[4];
                }
                else
                {
                    possiveisNomes = arquivoNomes[5];
                }
            }
            else if(raca == "Gnomo")
            {
                if (generoNome.GetComponent<Slider>().value == 1)
                {
                    possiveisNomes = arquivoNomes[6];
                }
                else
                {
                    possiveisNomes = arquivoNomes[7];
                }
            }
            else if(raca == "Halfling")
            {
                if (generoNome.GetComponent<Slider>().value == 1)
                {
                    possiveisNomes = arquivoNomes[8];
                }
                else
                {
                    possiveisNomes = arquivoNomes[9];
                }
            }
            else  if(raca == "Humano")
            {
                if (generoNome.GetComponent<Slider>().value == 1)
                {
                    possiveisNomes = arquivoNomes[14];
                }
                else
                {
                    possiveisNomes = arquivoNomes[15];
                }
            }
            else if(raca == "Meio-Elfo")
            {
                int ra = Random.Range(0, 2);
                if(ra == 0)
                {
                    if (generoNome.GetComponent<Slider>().value == 1)
                    {
                        possiveisNomes = arquivoNomes[14];
                    }
                    else
                    {
                        possiveisNomes = arquivoNomes[15];
                    }
                }
                else
                {
                    if (generoNome.GetComponent<Slider>().value == 1)
                    {
                        possiveisNomes = arquivoNomes[4];
                    }
                    else
                    {
                        possiveisNomes = arquivoNomes[5];
                    }
                }
            }
            else if(raca == "Meio-Orc")
            {
                if (generoNome.GetComponent<Slider>().value == 1)
                {
                    possiveisNomes = arquivoNomes[10];
                }
                else
                {
                    possiveisNomes = arquivoNomes[11];
                }
            }
            else if(raca == "Tiefling")
            {
                if (generoNome.GetComponent<Slider>().value == 1)
                {
                    possiveisNomes = arquivoNomes[12];
                }
                else
                {
                    possiveisNomes = arquivoNomes[13];
                }
            }
            string[] possiveisNomes_ = possiveisNomes.Split(", ");
            int rando = Random.Range(0, possiveisNomes_.Length);
            textoNome.text = possiveisNomes_[rando];
        }
        else
        {
            textoNome.text = nome.text;
        }


        string path = Path.Combine(Application.streamingAssetsPath, arquivosRacas[raca]);
        string[] linhas = File.ReadAllLines(path, System.Text.Encoding.UTF7);
        int contadorLinha = 0;
        int textoHabilidade = 0;
        int textoIdioma = 0;
        int textoPericia = 0;
        int textoTruque = 0;
        int esperaSubClasse = 0;
        int contadorRegressivo = 9999;
        foreach (string linha in linhas)
        {
            contadorLinha++;
            if(contadorRegressivo > 0)
            {
                if (textoHabilidade > 0)
                {
                    habilidades.Add(linha);
                    textoHabilidade--;
                }
                else if(textoTruque > 0)
                {
                    string cod = linha.Substring(0, 4);
                    if(cod == "CERT")
                    {
                        string n = linha.Substring(5, 1);
                        if(int.TryParse(n, out int num))
                        {
                            if(nivel >= num)
                            {
                                string truque = linha.Substring(7);
                                truques.Add(truque);
                            }
                        }
                        else
                        {
                            string truque = linha.Substring(5);
                            truques.Add(truque);
                        }
                    }
                    else if(cod == "MAGO")
                    {
                        string path1 = Path.Combine(Application.streamingAssetsPath, "Texts/Raças/TruquesMago.txt");
                        string[] truquesMago = File.ReadAllLines(path1, System.Text.Encoding.UTF7);
                        string truque = truquesMago[Random.Range(0, truquesMago.Length)];
                        bool repetiu = true;
                        while (repetiu)
                        {
                            repetiu = false;
                            for (int i = 0; i < truques.Count; i++)
                            {
                                if (truques[i] == truque)
                                {
                                    repetiu = true;
                                }
                            }
                            if (repetiu)
                            {
                                truque = truquesMago[Random.Range(0, truquesMago.Length)];
                            }
                        }
                        truques.Add(truque);
                        
                    }
                }
                else if(textoPericia > 0)
                {
                    if (linha == "Aleatorio")
                    {
                        int n = Random.Range(0, 18);
                        while(pericias[n] == true)
                        {
                            n = Random.Range(0, 18);
                        }
                        pericias[n] = true;
                    }
                    else
                    {
                        pericias[periciasPos[linha]] = true;
                    }
                    textoPericia--;
                }
                else if (esperaSubClasse > 0)
                {
                    esperaSubClasse--;
                }
                else if (textoIdioma > 0)
                {
                    if (linha == "Aleatorio")
                    {
                        int n = Random.Range(0, 9);
                        idiomas.Add(idiomasDnD[n]);
                    }
                    else
                    {
                        idiomas.Add(linha);
                    }
                    textoIdioma--;
                }
                else
                {
                    string comando = linha.Substring(0, 4);
                    if (comando == "FORC")
                    {
                        string quanto = linha.Substring(5, 1);
                        atributos[0] += int.Parse(quanto);
                        
                    }
                    else if (comando == "DEST")
                    {
                        string quanto = linha.Substring(5, 1);
                        atributos[1] += int.Parse(quanto);
                    }
                    else if (comando == "CONS")
                    {
                        string quanto = linha.Substring(5, 1);
                        atributos[2] += int.Parse(quanto);
                    }
                    else if (comando == "INTE")
                    {
                        string quanto = linha.Substring(5, 1);
                        atributos[3] += int.Parse(quanto);
                    }
                    else if (comando == "SABE")
                    {
                        string quanto = linha.Substring(5, 1);
                        atributos[4] += int.Parse(quanto);
                    }
                    else if (comando == "CARI")
                    {
                        string quanto = linha.Substring(5, 1);
                        atributos[5] += int.Parse(quanto);
                    }
                    else if (comando == "DESL")
                    {
                        string des = linha.Substring(5, 1);
                        deslocamento = int.Parse(des);
                        deslocamentoTexto.text = deslocamento.ToString();
                    }
                    else if (comando == "HABI")
                    {
                        string n = linha.Substring(5, 1);
                        textoHabilidade = int.Parse(n);
                    }
                    else if (comando == "IDIO")
                    {
                        string n = linha.Substring(5, 1);
                        textoIdioma = int.Parse(n);
                    }
                    else if (comando == "SUBC")
                    {
                        if(qualSubRaca  == -1)
                        {
                            qualSubRaca = Random.Range(0, quantasSubRacas[raca]);
                        }
                        string com = linha.Substring(5 + (qualSubRaca * 6), 2);
                        string fim = linha.Substring(8 + (qualSubRaca * 6), 2);
                        esperaSubClasse = int.Parse(com) - contadorLinha - 1;
                        contadorRegressivo = int.Parse(fim) - contadorLinha + 1;
                    }
                    else if(comando == "PROF")
                    {
                        string n = linha.Substring(5, 1);
                        textoPericia = int.Parse(n);
                    }
                    else if(comando == "TRUQ")
                    {
                        string n = linha.Substring(5, 1);
                        textoTruque = int.Parse(n);
                    }
                }
            }
            contadorRegressivo--;

        }
    }

    private void ColocaModificadoresNaFicha()
    {
        int[] modificadores = { 0, 0, 0, 0, 0, 0 };
        for(int i = 0; i < 6; i++)
        {
            modificadores[i] = (int)(atributos[i] / 2)-5;
        }
        modForca.text = modificadores[0].ToString();
        modDestreza.text = modificadores[1].ToString();
        iniciativaTexto.text = modificadores[1].ToString();
        modConstituicao.text = modificadores[2].ToString();
        modInteligencia.text = modificadores[3].ToString();
        modSabedoria.text = modificadores[4].ToString();
        modCarisma.text = modificadores[5].ToString();
    }

    private void ColocaAtributosNaFicha()
    {
        forca.text = atributos[0].ToString();
        destreza.text = atributos[1].ToString();
        constituicao.text = atributos[2].ToString();
        inteligencia.text = atributos[3].ToString();
        sabedoria.text = atributos[4].ToString();
        carisma.text = atributos[5].ToString();
    }

    private void ResetaAtributos()
    {
        int[] aatributos = { 15, 14, 13, 12, 10, 8 };
        aTRIBUTOS = aatributos;
    }

    private void EscolherAtributosFaltando(int[] escolhidos)
    {
        int[] posicoes = { 0, 1, 2, 3, 4, 5 };
        posicoes = posicoes.Where(n => n != escolhidos[0]).ToArray();
        posicoes = posicoes.Where(n => n != escolhidos[1]).ToArray();
        System.Random rnd = new System.Random();
        int[] novosAtributos = aTRIBUTOS.OrderBy(x => rnd.Next()).ToArray();
        atributos[posicoes[0]] = novosAtributos[0];
        atributos[posicoes[1]] = novosAtributos[1];
        atributos[posicoes[2]] = novosAtributos[2];
        atributos[posicoes[3]] = novosAtributos[3];
    }

    private void EscolheAtributoPrincipal(int qual)
    {
        int n = Random.Range(1, 22);
        if (n <= 6)
        {
            atributos[qual] = aTRIBUTOS[0];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[0]).ToArray();
        }
        else if (n <= 11)
        {
            atributos[qual] = aTRIBUTOS[1];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[1]).ToArray();
        }
        else if (n <= 15)
        {
            atributos[qual] = aTRIBUTOS[2];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[2]).ToArray();
        }
        else if (n <= 18)
        {
            atributos[qual] = aTRIBUTOS[3];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[3]).ToArray();
        }
        else if (n <= 20)
        {
            atributos[qual] = aTRIBUTOS[4];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[4]).ToArray();
        }
        else
        {
            atributos[qual] = aTRIBUTOS[5];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[5]).ToArray();
        }
    }

    private void EscolheAtributoSecundario(int qual)
    {
        int n = Random.Range(1, 16);
        if (n <= 5)
        {
            atributos[qual] = aTRIBUTOS[0];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[0]).ToArray();
        }
        else if (n <= 9)
        {
            atributos[qual] = aTRIBUTOS[1];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[1]).ToArray();
        }
        else if (n <= 12)
        {
            atributos[qual] = aTRIBUTOS[2];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[2]).ToArray();
        }
        else if (n <= 14)
        {
            atributos[qual] = aTRIBUTOS[3];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[3]).ToArray();
        }
        else
        {
            atributos[qual] = aTRIBUTOS[4];
            aTRIBUTOS = aTRIBUTOS.Where(n => n != aTRIBUTOS[4]).ToArray();
        }
    }

    // Update is called once per frame
}
