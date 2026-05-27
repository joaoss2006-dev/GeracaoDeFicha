using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Ladino : MonoBehaviour
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
        { 3, new int[] { 3, 3, 2, 0, 0, 0 } },
        { 4, new int[] { 3, 4, 3, 0, 0, 0 } },
        { 5, new int[] { 3, 4, 3, 0, 0, 0 } },
        { 6, new int[] { 3, 4, 3, 0, 0, 0 } },
        { 7, new int[] { 3, 5, 4, 2, 0, 0 } },
        { 8, new int[] { 3, 6, 4, 2, 0, 0 } },
        { 9, new int[] { 3, 6, 4, 2, 0, 0 } },
        { 10, new int[] { 4, 7, 4, 3, 0, 0 } },
        { 11, new int[] { 4, 8, 4, 3, 0, 0 } },
        { 12, new int[] { 4, 8, 4, 3, 0, 0 } },
        { 13, new int[] { 4, 9, 4, 3, 2, 0 } },
        { 14, new int[] { 4, 10, 4, 3, 2, 0 } },
        { 15, new int[] { 4, 10, 4, 3, 2, 0 } },
        { 16, new int[] { 4, 11, 4, 3, 3, 0 } },
        { 17, new int[] { 4, 11, 4, 3, 3, 0 } },
        { 18, new int[] { 4, 11, 4, 3, 3, 0 } },
        { 19, new int[] { 4, 12, 4, 3, 3, 1 } },
        { 20, new int[] { 4, 13, 4, 3, 3, 1 } }
    };

    public void LendoArquivo()
    {
        int nivel = com.PassaNivel();
        string path = Path.Combine(Application.streamingAssetsPath, "Texts\\Classes\\Ladino.txt");
        string[] linhas = File.ReadAllLines(path, System.Text.Encoding.UTF7);
        List<string> pericias = new List<string>() { };
        int opcoes = -1;
        int pegarQuantos = 0;
        int esperarSub = 0;
        int contadorLinha = 0;
        int quantasLinhasSub = 0;
        int truques = 0;
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
            //Verificando e adicionando os truques
            else if (truques > 0)
            {
                string cod = linha.Substring(0, 4);
                if (cod == "MAGO")
                {
                    string path1 = Path.Combine(Application.streamingAssetsPath, "Texts\\Raças\\TruquesMago.txt");
                    string[] truquesMago = File.ReadAllLines(path1, System.Text.Encoding.UTF7);
                    int indice = Random.Range(0, truquesMago.Length);
                    int contador = 0;
                    while (com.EstaNosTruques(truquesMago[indice]))
                    {
                        indice = Random.Range(0, truquesMago.Length);
                        if (contador >= 20)
                        {
                            break;
                        }
                    }
                }
                truques--;
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
                        opcoes = int.Parse(linha.Substring(5, 2));
                        pegarQuantos = int.Parse(linha.Substring(8, 2));
                        break;
                    //Analizando a sub-Classe
                    case "SuCl":
                        int subCl = cla.PassaSubClasse();
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
                        if (nivelRequisito <= nivel)
                        {
                            
                            //Caso haja nivel para adiquirir a habilidade
                            //Verificando se é algum código para algo especifico
                            string codigo = linha.Substring(5, 4);
                            if (codigo == "MAGI")
                            {
                                com.AplicaCDMagias(3);
                                int[] tabela = TabelaMagia[nivel];
                                //Pegando os Truques da tabela
                                for (int i = 0; i < tabela[0]; i++)
                                {
                                    string path2 = Path.Combine(Application.streamingAssetsPath, "Texts\\Raças\\TruquesMago.txt");
                                    string[] truquesMago = File.ReadAllLines(path2, System.Text.Encoding.UTF7);
                                    int indice = Random.Range(0, truquesMago.Length);
                                    int contador = 0;
                                    while (com.EstaNosTruques(truquesMago[indice]))
                                    {
                                        indice = Random.Range(0, truquesMago.Length);
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
                                        if (tabela[5] > 0) { nivelMagias++; }
                                    }
                                }

                                //pegando as magias
                                for (int i = 0; i < tabela[1]; i++)
                                {
                                    int qualTabela = Random.Range(1, nivelMagias + 1);
                                    string path3 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\Mago{qualTabela}.txt");
                                    string[] magias = File.ReadAllLines(path3, System.Text.Encoding.UTF7);
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
                            }
                            else
                            {
                                //Caso não seja código para nada, só adiciona a habilidade
                                com.AdicionaHabilidadeClasse(linha.Substring(5));
                            }
                        }
                        break;
                }
            }

        }
        int quantosDados;
        if(nivel%2 == 0)
        {
            quantosDados = nivel / 2;
        }
        else
        {
            quantosDados = nivel / 2 + 1;
        }
        string texto = $"{quantosDados}d6 de Ataque Furtivo";
        cla.PegaEspecificidade(texto);

    }

}
