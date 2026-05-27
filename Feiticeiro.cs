using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Feiticeiro : MonoBehaviour
{
    private Controller com;
    private Classes cla;
    void Start()
    {
        com = FindObjectOfType<Controller>();
        cla = FindObjectOfType<Classes>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private static readonly Dictionary<int, int[]> TabelaMagia = new()
    {
        { 1, new int[] { 4, 2, 2, 0, 0, 0, 0, 0, 0, 0, 0 } },
        { 2, new int[] { 4, 3, 3, 0, 0, 0, 0, 0, 0, 0, 0 } },
        { 3, new int[] { 4, 4, 4, 2, 0, 0, 0, 0, 0, 0, 0 } },
        { 4, new int[] { 5, 5, 4, 3, 0, 0, 0, 0, 0, 0, 0 } },
        { 5, new int[] { 5, 6, 4, 3, 2, 0, 0, 0, 0, 0, 0 } },
        { 6, new int[] { 5, 7, 4, 3, 3, 0, 0, 0, 0, 0, 0 } },
        { 7, new int[] { 5, 8, 4, 3, 3, 1, 0, 0, 0, 0, 0 } },
        { 8, new int[] { 5, 9, 4, 3, 3, 2, 0, 0, 0, 0, 0 } },
        { 9, new int[] { 5, 10, 4, 3, 3, 3, 1, 0, 0, 0, 0 } },
        { 10, new int[] { 6, 11, 4, 3, 3, 3, 2, 0, 0, 0, 0 } },
        { 11, new int[] { 6, 12, 4, 3, 3, 3, 2, 1, 0, 0, 0 } },
        { 12, new int[] { 6, 12, 4, 3, 3, 3, 2, 1, 0, 0, 0 } },
        { 13, new int[] { 6, 13, 4, 3, 3, 3, 2, 1, 1, 0, 0 } },
        { 14, new int[] { 6, 13, 4, 3, 3, 3, 2, 1, 1, 0, 0 } },
        { 15, new int[] { 6, 14, 4, 3, 3, 3, 2, 1, 1, 1, 0 } },
        { 16, new int[] { 6, 14, 4, 3, 3, 3, 2, 1, 1, 1, 0 } },
        { 17, new int[] { 6, 15, 4, 3, 3, 3, 2, 1, 1, 1, 1 } },
        { 18, new int[] { 6, 15, 4, 3, 3, 3, 3, 1, 1, 1, 1 } },
        { 19, new int[] { 6, 15, 4, 3, 3, 3, 3, 2, 1, 1, 1 } },
        { 20, new int[] { 6, 15, 4, 3, 3, 3, 3, 2, 2, 1, 1 } }
    };

    public void LendoArquivo()
    {
        int nivel = com.PassaNivel();
        string path = Path.Combine(Application.streamingAssetsPath, "Texts\\Classes\\Feiticeiro.txt");
        string[] linhas = File.ReadAllLines(path, System.Text.Encoding.UTF7);
        List<string> pericias = new List<string>() { };
        int opcoes = -1;
        int pegarQuantos = 0;
        int esperarSub = 0;
        int contadorLinha = 0;
        int quantasLinhasSub = 0;
        bool noSub = false;
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
                        int subCl = cla.PassaSubClasse();
                        if (subCl == -1) { subCl = Random.Range(0, 2); }
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
                                //===========================================================================================================================================================
                                com.AplicaCDMagias(5);
                                //===========================================================================================================================================================
                                int[] tabela = TabelaMagia[nivel];
                                //Pegando os Truques da tabela
                                for (int i = 0; i < tabela[0]; i++)
                                {
                                    string path1 = Path.Combine(Application.streamingAssetsPath, "Texts\\Magias\\TruquesFeiticeiro.txt");
                                    string[] truques = File.ReadAllLines(path1, System.Text.Encoding.UTF7);
                                    int indice = Random.Range(0, truques.Length);
                                    int contador = 0;
                                    while (com.EstaNosTruques(truques[indice]))
                                    {
                                        indice = Random.Range(0, truques.Length);
                                        if (contador >= 20)
                                        {
                                            break;
                                        }
                                    }
                                }
                                //Verificando até que nível posso pegar as magias
                                int nivelMagias = 1;
                                if (tabela[3] > 0)
                                {
                                    nivelMagias++;
                                    if (tabela[4] > 0)
                                    {
                                        nivelMagias++;
                                        if (tabela[5] > 0) 
                                        {
                                            nivelMagias++;
                                            if (tabela[6] > 0)
                                            {
                                                nivelMagias++;
                                                if (tabela[7] > 0)
                                                {
                                                    nivelMagias++;
                                                    if (tabela[8] > 0)
                                                    {
                                                        nivelMagias++;
                                                        if (tabela[9] > 0)
                                                        {
                                                            nivelMagias++;
                                                            if (tabela[10] > 0)
                                                            {
                                                                nivelMagias++;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //pegando as magias
                                for (int i = 0; i < tabela[1]; i++)
                                {
                                    int qualTabela = Random.Range(1, nivelMagias + 1);
                                    string path2 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\Feiticeiro{qualTabela}.txt");
                                    string[] magias = File.ReadAllLines(path2, System.Text.Encoding.UTF7);
                                    int rand = Random.Range(0, magias.Length);
                                    //garantindo que não vá ficar preso no while
                                    int contador = 0;
                                    while (com.TentaAplicarMagia(magias[rand], qualTabela))
                                    {
                                        rand = Random.Range(0, magias.Length);
                                        if (contador > 20)
                                        {
                                            break;
                                        }
                                        contador++;
                                    }
                                }
                                //passando a quantidade de espaços de magia
                                com.ColocaEspacosMagia(tabela[2], 1);
                                com.ColocaEspacosMagia(tabela[3], 2);
                                com.ColocaEspacosMagia(tabela[4], 3);
                                com.ColocaEspacosMagia(tabela[5], 4);
                                com.ColocaEspacosMagia(tabela[6], 5);
                                com.ColocaEspacosMagia(tabela[7], 6);
                                com.ColocaEspacosMagia(tabela[8], 7);
                                com.ColocaEspacosMagia(tabela[9], 8);
                                com.ColocaEspacosMagia(tabela[10], 9);
                            }
                            else if(codigo == "META")
                            {
                                List<int> jaPegos = new List<int>() { };
                                int quantos = 2;
                                if(nivel >= 10)
                                {
                                    quantos++;
                                    if (nivel >= 17)
                                    {
                                        quantos++;
                                    }
                                }
                                for (int i = 0; i < quantos; i++)
                                {
                                    int rand = Random.Range(0, 8);
                                    int contador = 0;
                                    while (jaPegos.Contains(rand))
                                    {
                                        rand = Random.Range(0, 8);
                                        if (contador > 20)
                                        {
                                            rand = -1;
                                            break;
                                        }
                                    }
                                    if (rand != -1)
                                    {
                                        jaPegos.Add(rand);
                                    }
                                    string text = "";
                                    switch (rand)
                                    {
                                        case 0:
                                            text = "Magia Acelerada";
                                            break;
                                        case 1:
                                            text = "Magia Aumentada";
                                            break;
                                        case 2:
                                            text = "Magia Cuidadosa";
                                            break;
                                        case 3:
                                            text = "Magia Distante";
                                            break;
                                        case 4:
                                            text = "Magia Duplicada";
                                            break;
                                        case 5:
                                            text = "Magia Estendida";
                                            break;
                                        case 6:
                                            text = "Magia Potencializada";
                                            break;
                                        case 7:
                                            text = "Magia Sutil";
                                            break;
                                    }
                                    com.AdicionaHabilidadeClasse(text);
                                }
                            }
                            else if(codigo == "REDR")
                            {
                                com.AdicionaVida(nivel);
                                com.PegaClasseArmadura(13, true, false);
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
        string texto = "";
        if(nivel >= 2)
        {
            texto = $"Pontos de Feitiçaria: {nivel}";
        }
        cla.PegaEspecificidade(texto);
        
    }
}
