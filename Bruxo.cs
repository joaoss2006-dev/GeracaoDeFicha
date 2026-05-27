using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Bruxo : MonoBehaviour
{
    private Controller com;
    private Classes cla;
    void Start()
    {
        com = FindObjectOfType<Controller>();
        cla = FindObjectOfType<Classes>();
    }



    private static readonly Dictionary<int, int[]> TabelaMagia = new()
    {
        { 1, new int[] { 2, 2, 1, 1 } },
        { 2, new int[] { 2, 3, 2, 1 } },
        { 3, new int[] { 2, 4, 2, 2 } },
        { 4, new int[] { 3, 5, 2, 2 } },
        { 5, new int[] { 3, 6, 2, 3 } },
        { 6, new int[] { 3, 7, 2, 3 } },
        { 7, new int[] { 3, 8, 2, 4 } },
        { 8, new int[] { 3, 9, 2, 4 } },
        { 9, new int[] { 3, 10, 2, 5 } },
        { 10, new int[] { 4, 10, 2, 5 } },
        { 11, new int[] { 4, 11, 3, 5 } },
        { 12, new int[] { 4, 11, 3, 5 } },
        { 13, new int[] { 4, 12, 3, 5 } },
        { 14, new int[] { 4, 12, 3, 5 } },
        { 15, new int[] { 4, 13, 3, 5 } },
        { 16, new int[] { 4, 13, 3, 5 } },
        { 17, new int[] { 4, 14, 4, 5 } },
        { 18, new int[] { 4, 14, 4, 5 } },
        { 19, new int[] { 4, 15, 4, 5 } },
        { 20, new int[] { 4, 15, 4, 5 } }
    };

    private List<string> InvocacoesMisticas(int nivel, int qualPacto, bool rajadaMistica)
    {
        List<string> invocacoesMisticas = new List<string>() { };
        invocacoesMisticas.Add("Armadura de Sombras");
        invocacoesMisticas.Add("Idioma Bestial");
        invocacoesMisticas.Add("Influência Enganadora");
        invocacoesMisticas.Add("Larápio dos Cinco Destinos");
        invocacoesMisticas.Add("Máscara das Muitas Faces");
        invocacoesMisticas.Add("Olhar de Duas Mentes");
        invocacoesMisticas.Add("Olhos do Guardião das Runas");
        invocacoesMisticas.Add("Vigor Abissal");
        invocacoesMisticas.Add("Visão Diabólica");
        invocacoesMisticas.Add("Visão Mística");
        invocacoesMisticas.Add("Visões nas Brumas");
        if (nivel >= 5)
        {
            invocacoesMisticas.Add("Encharcar a Mente");
            invocacoesMisticas.Add("Sinal de Mau Agouro");
            invocacoesMisticas.Add("Uno com as Sombras");
            if (qualPacto == 1) { invocacoesMisticas.Add("Lâmina Sedenta"); }
            if(nivel >= 7)
            {
                invocacoesMisticas.Add("Escultor de Carne");
                invocacoesMisticas.Add("Palavra Terrível");
                invocacoesMisticas.Add("Sussurros Sedutores");
                if(nivel >= 9)
                {
                    invocacoesMisticas.Add("Lacaios do Caos");
                    invocacoesMisticas.Add("Passo Ascendente");
                    invocacoesMisticas.Add("Salto Transcedental");
                    invocacoesMisticas.Add("Sussurros da Sepultura");
                    if(nivel >= 12)
                    {
                        if(qualPacto == 1) { invocacoesMisticas.Add("Sorvedor de Vida"); }
                        if(nivel >= 15)
                        {
                            invocacoesMisticas.Add("Correntes de Cárceri");
                            invocacoesMisticas.Add("Mestre das Infindáveis Formas");
                            invocacoesMisticas.Add("Visão da Bruxa");
                            invocacoesMisticas.Add("Visões de Reinos Distantes");
                        }
                    }
                }
            }
        }
        if (rajadaMistica)
        {
            invocacoesMisticas.Add("Explosão Agonizante");
            invocacoesMisticas.Add("Explosão Repulsiva");
            invocacoesMisticas.Add("Lança Mística");
        }
        if (qualPacto == 0) { invocacoesMisticas.Add("Voz do Mestre das Correntes"); }
        if (qualPacto == 2) { invocacoesMisticas.Add("Livro de Segredos Antigos");}
        return invocacoesMisticas;
    }

    public void LendoArquivo(int pacto)
    {
        int nivel = com.PassaNivel();
        string path = Path.Combine(Application.streamingAssetsPath, "Texts/Classes/Bruxo.txt");
        string[] linhas = File.ReadAllLines(path, System.Text.Encoding.UTF7);
        List<string> pericias = new List<string>() { };
        int opcoes = -1;
        int pegarQuantos = 0;
        int esperarSub = 0;
        int contadorLinha = 0;
        int quantasLinhasSub = 0;
        bool noSub = false;
        int pacto_;
        bool rajadaMistica = false;
        int subCl = cla.PassaSubClasse();
        List<string> invocacoesMisticas = new List<string>() { };
        List<List<string>> magiasAdicionais = new List<List<string>>() { new List<string>() { }, new List<string>() { }, new List<string>() { }, new List<string>() { }, new List<string>() { }};
        if (pacto == -1)
        {
            pacto_ = Random.Range(0, 3);
        }
        else
        {
            pacto_ = pacto;
        }
        foreach (string linha in linhas)
        {
            contadorLinha++;
            //Verificando se terminou de ver a sub-classe
            if (noSub)
            {
                if (quantasLinhasSub <= 0)
                {
                    break;
                }
                quantasLinhasSub--;
            }
            //Iguinorando coisas antes de chegar a sub-classe
            if (esperarSub > 0)
            {
                if (esperarSub == 1)
                {

                    noSub = true;
                }
                esperarSub--;
            }
            //Verificando as opções de pericias
            else if (opcoes > 0)
            {
                pericias.Add(linha);
                opcoes--;
                if (opcoes == 0)
                {
                    for (int i = 0; i < pegarQuantos; i++)
                    {
                        int contador = 0;
                        int rand = Random.Range(0, pericias.Count);
                        while (com.TentaAplicarPericia(pericias[rand]) == 0)
                        {
                            rand = Random.Range(0, pericias.Count);
                            contador++;
                            if (contador >= pegarQuantos * 3)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            //Sequencia principal do programa
            else
            {
                string cod = linha.Substring(0, 4);
                switch (cod)
                {
                    //Analizando as proficiências em pericias
                    case "PROF":
                        opcoes = int.Parse(linha.Substring(5, 1));
                        pegarQuantos = int.Parse(linha.Substring(7, 1));
                        break;
                    //Analizando a sub-Classe
                    case "SuCl":
                        if (subCl == -1) { subCl = Random.Range(0, 3); }
                        int comeco = int.Parse(linha.Substring(5 + 6 * subCl, 2));
                        int final = int.Parse(linha.Substring(8 + 6 * subCl, 2));
                        esperarSub = (comeco - contadorLinha) - 1;
                        quantasLinhasSub = (final - comeco) + 1;
                        break;
                    //Analizando a armadura e classe de armadura
                    case "DURA":
                        int qualArmadura = int.Parse(linha.Substring(5, 1));
                        switch (qualArmadura)
                        {
                            case 0:
                                com.PegaClasseArmadura(10, true, false);
                                break;
                            case 1:
                                int rand = Random.Range(0, 3);
                                if (rand == 0) { com.PegaClasseArmadura(11, true, false); }
                                else if (rand == 1) { com.PegaClasseArmadura(11, true, false); }
                                else if (rand == 2) { com.PegaClasseArmadura(12, true, false); }
                                com.QualArmadura("Armadura Leve");
                                break;
                            case 2:
                                int rand1 = Random.Range(0, 5);
                                if (rand1 == 0) { com.PegaClasseArmadura(12, true, true); }
                                else if (rand1 == 1) { com.PegaClasseArmadura(13, true, true); }
                                else if (rand1 == 2) { com.PegaClasseArmadura(14, true, true); }
                                else if (rand1 == 3) { com.PegaClasseArmadura(14, true, true); }
                                else if (rand1 == 4) { com.PegaClasseArmadura(15, true, true); }
                                com.QualArmadura("Armadura Média");
                                break;
                            case 3:
                                int rand2 = Random.Range(0, 4);
                                if (rand2 == 0) { com.PegaClasseArmadura(14, false, true); }
                                else if (rand2 == 1) { com.PegaClasseArmadura(16, false, true); }
                                else if (rand2 == 2) { com.PegaClasseArmadura(17, false, true); }
                                else if (rand2 == 3) { com.PegaClasseArmadura(18, false, true); }
                                com.QualArmadura("Armadura Pesada");
                                break;
                        }
                        break;
                    //Caso não seja nenhum código, ou seja, é um número
                    default:
                        int nivelRequisito;
                        if (int.TryParse(cod, out int niRe))
                        {
                            nivelRequisito = niRe;
                        }
                        else
                        {
                            nivelRequisito = 9999;
                        }
                        //Caso haja nivel para adiquirir a habilidade
                        if (nivelRequisito <= nivel)
                        {
                            //Verificando se é algum código para algo especifico
                            string codigo = linha.Substring(5, 4);
                            if (codigo == "MAGI")
                            {
                                com.AplicaCDMagias(5);
                                int[] tabela = TabelaMagia[nivel];
                                //Pegando os Truques da tabela
                                for (int i = 0; i < tabela[0]; i++)
                                {
                                    string path1 = Path.Combine(Application.streamingAssetsPath, "Texts/Magias/TruquesBruxo.txt");
                                    string[] truquesBruxo = File.ReadAllLines(path1, System.Text.Encoding.UTF7);
                                    int indice = Random.Range(0, truquesBruxo.Length);
                                    int contador = 0;
                                    if(truquesBruxo[indice] == "Rajada Mística (evocação)") { rajadaMistica = true; }
                                    while (com.EstaNosTruques(truquesBruxo[indice]))
                                    {
                                        indice = Random.Range(0, truquesBruxo.Length);
                                        if (truquesBruxo[indice] == "Rajada Mística (evocação)") { rajadaMistica = true; }
                                        if (contador >= 20)
                                        {
                                            break;
                                        }
                                    }
                                }
                                //Verificando até que nível posso pegar as magias
                                int nivelMagias = tabela[3];
                                
                                
                                //pegando as magias
                                for (int i = 0; i < tabela[1]; i++)
                                {
                                    int qualTabela = Random.Range(1, tabela[3] + 1);
                                    string path2 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\Bruxo{qualTabela}.txt");
                                    string[] magias = File.ReadAllLines(path2, System.Text.Encoding.UTF7);
                                    List<string> magiasB = new List<string>() { };
                                    for(int j = 0; j < magias.Length; j++)
                                    {
                                        magiasB.Add(magias[j]);
                                    }
                                    for(int j = 0; j < magiasAdicionais[tabela[3]-1].Count; j++)
                                    {
                                        magiasB.Add(magiasAdicionais[tabela[3] - 1][j]);
                                    }
                                    int rand = Random.Range(0, magiasB.Count);
                                    //garantindo que não vá ficar preso no while
                                    int contador = 0;
                                    while (com.TentaAplicarMagia(magiasB[rand], qualTabela))
                                    {
                                        rand = Random.Range(0, magiasB.Count);
                                        if (contador > 20)
                                        {
                                            break;
                                        }
                                        contador++;
                                    }
                                }
                                //passando a quantidade de espaços de magia
                                com.ColocaEspacosMagia(tabela[2], tabela[3]);

                            }
                            else if(codigo == "InMi")
                            {
                                int quantas = 2;
                                if(nivel >= 5)
                                {
                                    quantas++;
                                    if(nivel >= 7)
                                    {
                                        quantas++;
                                        if(nivel >= 9)
                                        {
                                            quantas++;
                                            if(nivel >= 12)
                                            {
                                                quantas++;
                                                if(nivel >= 15)
                                                {
                                                    quantas++;
                                                    if(nivel >= 18)
                                                    {
                                                        quantas++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                ////////////////////////////////////////////////////////////////////
                                //Trocar o 0 que diz respeito ao pacto
                                
                                
                                List<string> quaisPossoPegar = InvocacoesMisticas(nivel, pacto_, rajadaMistica);
                                for(int i = 0; i < quantas; i++)
                                {
                                    int rand = Random.Range(0, quaisPossoPegar.Count);
                                    com.AdicionaHabilidadeClasse(quaisPossoPegar[rand]);
                                    quaisPossoPegar.RemoveAt(rand);
                                }
                            }
                            else if(codigo == "DDPa")
                            {
                                switch (pacto_)
                                {
                                    case 0:
                                        com.AdicionaHabilidadeClasse("Pacto da Corrente");
                                        break;
                                    case 1:
                                        com.AdicionaHabilidadeClasse("Pacto da Lâmina");
                                        break;
                                    case 2:
                                        com.AdicionaHabilidadeClasse("Pacto do Tomo");
                                        break;
                                }
                            }
                            else if(codigo == "ArMi")
                            {
                                string texto = "Arcana Mística:\n";
                                string path3 = Path.Combine(Application.streamingAssetsPath, "Texts\\Magias\\Bruxo6.txt");
                                string[] magias6 = File.ReadAllLines(path3, System.Text.Encoding.UTF7);
                                int rand = Random.Range(0, magias6.Length);
                                texto += magias6[rand];
                                if(nivel >= 13)
                                {
                                    string path4 = Path.Combine(Application.streamingAssetsPath, "Texts\\Magias\\Bruxo7.txt");
                                    string[] magias7 = File.ReadAllLines(path4, System.Text.Encoding.UTF7);
                                    int rand2 = Random.Range(0, magias7.Length);
                                    texto += $"\n{magias7[rand2]}";
                                    if(nivel >= 15)
                                    {
                                        string path5 = Path.Combine(Application.streamingAssetsPath, "Texts\\Magias\\Bruxo8.txt");
                                        string[] magias8 = File.ReadAllLines(path5, System.Text.Encoding.UTF7);
                                        int rand3 = Random.Range(0, magias8.Length);
                                        texto += $"\n{magias8[rand3]}";
                                        if(nivel >= 17)
                                        {
                                            string path6 = Path.Combine(Application.streamingAssetsPath, "Texts\\Magias\\Bruxo9.txt");
                                            string[] magias9 = File.ReadAllLines(path6, System.Text.Encoding.UTF7);
                                            int rand4 = Random.Range(0, magias9.Length);
                                            texto += $"\n{magias9[rand4]}";
                                        }
                                    }
                                }
                                cla.PegaEspecificidade(texto);
                            }
                            else if(codigo == "LMEx")
                            {
                                switch (subCl)
                                {
                                    case 0:
                                        magiasAdicionais[0].Add("Fogo das Fadas");
                                        magiasAdicionais[0].Add("Sono");
                                        magiasAdicionais[1].Add("Acalmar Emoções");
                                        magiasAdicionais[1].Add("Força Fantasmagórica");
                                        magiasAdicionais[2].Add("Piscar");
                                        magiasAdicionais[2].Add("Ampliar Plantas");
                                        magiasAdicionais[3].Add("Dominar Besta");
                                        magiasAdicionais[3].Add("Invisibilidade Maior");
                                        magiasAdicionais[4].Add("Dominar Pessoa");
                                        magiasAdicionais[4].Add("Similaridade");
                                        break;
                                    case 1:
                                        magiasAdicionais[0].Add("Mãos Flamejantes");
                                        magiasAdicionais[0].Add("Comando");
                                        magiasAdicionais[1].Add("Cegueira/Surdez");
                                        magiasAdicionais[1].Add("Raio Ardente");
                                        magiasAdicionais[2].Add("Bola de Fogo");
                                        magiasAdicionais[2].Add("Névoa Fétida");
                                        magiasAdicionais[3].Add("Escudo de Fogo");
                                        magiasAdicionais[3].Add("Muralha de Fogo");
                                        magiasAdicionais[4].Add("Coluna de Chamas");
                                        magiasAdicionais[4].Add("Consagrar");
                                        break;
                                    case 2:
                                        magiasAdicionais[0].Add("Sussuros Dissonantes");
                                        magiasAdicionais[0].Add("Riso Histérico de Tasha");
                                        magiasAdicionais[1].Add("Detectar Pensamentos");
                                        magiasAdicionais[1].Add("Força Fantasmagórica");
                                        magiasAdicionais[2].Add("Clarividência");
                                        magiasAdicionais[2].Add("Enviar Mensagem");
                                        magiasAdicionais[3].Add("Dominar Besta");
                                        magiasAdicionais[3].Add("Tentáculos Negros de Evard");
                                        magiasAdicionais[4].Add("Dominar Pessoa");
                                        magiasAdicionais[4].Add("Telecinésia");
                                        break;
                                }
                            }
                            //Caso não seja código para nada, só adiciona a habilidade
                            else
                            {
                                com.AdicionaHabilidadeClasse(linha.Substring(5));
                            }
                        }
                        break;
                }
            }

        }

    }
}
