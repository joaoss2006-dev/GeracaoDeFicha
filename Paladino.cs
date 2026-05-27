using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Paladino : MonoBehaviour
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
        { 2, new int[] {2, 0, 0, 0, 0 } },
        { 3, new int[] {3, 0, 0, 0, 0 } },
        { 4, new int[] {3, 0, 0, 0, 0 } },
        { 5, new int[] {4, 2, 0, 0, 0 } },
        { 6, new int[] {4, 2, 0, 0, 0 } },
        { 7, new int[] {4, 3, 0, 0, 0 } },
        { 8, new int[] {4, 3, 0, 0, 0 } },
        { 9, new int[] {4, 3, 2, 0, 0 } },
        { 10, new int[] {4, 3, 2, 0, 0 } },
        { 11, new int[] {4, 3, 3, 0, 0 } },
        { 12, new int[] {4, 3, 3, 0, 0 } },
        { 13, new int[] { 4, 3, 3, 1, 0 } },
        { 14, new int[] { 4, 3, 3, 1, 0 } },
        { 15, new int[] { 4, 3, 3, 2, 0 } },
        { 16, new int[] { 4, 3, 3, 2, 0 } },
        { 17, new int[] { 4, 3, 3, 3, 1 } },
        { 18, new int[] { 4, 3, 3, 3, 1 } },
        { 19, new int[] { 4, 3, 3, 3, 2 } },
        { 20, new int[] { 4, 3, 3, 3, 2 } }
    };

    public void LendoArquivo(int estiloDeLuta)
    {
        int nivel = com.PassaNivel();
        string path = Path.Combine(Application.streamingAssetsPath, "Texts\\Classes\\Paladino.txt");
        string[] linhas = File.ReadAllLines(path, System.Text.Encoding.UTF7);
        List<string> pericias = new List<string>() { };
        int opcoes = -1;
        int pegarQuantos = 0;
        int esperarSub = 0;
        int contadorLinha = 0;
        int quantasLinhasSub = 0;
        bool noSub = false;
        int subCl = cla.PassaSubClasse();
        if (subCl == -1) { subCl = Random.Range(0, 3); }
        List<string> divindade = new List<string>() { };
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

                                //Verificando até que nível posso pegar as magias
                                int nivelMagias = 1;
                                if (tabela[1] > 0)
                                {
                                    nivelMagias++;
                                    if (tabela[2] > 0)
                                    {
                                        nivelMagias++;
                                        if (tabela[3] > 0) 
                                        { 
                                            nivelMagias++; 
                                            if(tabela[4] > 0)
                                            {
                                                nivelMagias++;
                                            }
                                        }
                                    }
                                }

                                int quantas;
                                quantas = com.PassaModificadorAtributo(5) + (nivel / 2);

                                //pegando as magias
                                for (int i = 0; i < quantas; i++)
                                {
                                    int qualTabela = Random.Range(1, nivelMagias + 1);
                                    string path1 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\Paladino{qualTabela}.txt");
                                    string[] magias = File.ReadAllLines(path1, System.Text.Encoding.UTF7);
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
                                int cont = 1;
                                foreach(int item in tabela)
                                {
                                    com.ColocaEspacosMagia(item, cont);
                                    cont++;
                                }
                            }
                            else if(codigo == "CaDi")
                            {
                                string cadi = linha.Substring(10);
                                divindade.Add(cadi);
                            }
                            else if(codigo == "MaJu")
                            {
                                switch (subCl)
                                {
                                    case 0:
                                        com.TentaAplicarMagia("Proteção Contra o Bem e Mal (abjuração)", 1);
                                        com.TentaAplicarMagia("Santuário (abjuração)", 1);
                                        if(nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Restauração Menor (abjuração)", 2);
                                            com.TentaAplicarMagia("Zona da Verdade (encantamento)", 2);
                                            if(nivel >= 9)
                                            {
                                                com.TentaAplicarMagia("Sinal de Esperança (abjuração)", 3);
                                                com.TentaAplicarMagia("Dissipar Magia (abjuração)", 3);
                                                if(nivel >= 13)
                                                {
                                                    com.TentaAplicarMagia("Movimentação Livre (abjuração)", 4);
                                                    com.TentaAplicarMagia("Guardião da Fé (conjuração)", 4);
                                                    if(nivel >= 17)
                                                    {
                                                        com.TentaAplicarMagia("Comunhão (adivinhação)", 5);
                                                        com.TentaAplicarMagia("Coluna de Chamas (evocação)", 5);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 1:
                                        com.TentaAplicarMagia("Golpe Constritor (conjuração)", 1);
                                        com.TentaAplicarMagia("Falar com Animais (adivinhação)", 1);
                                        if (nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Raio Lunar (evocação)", 2);
                                            com.TentaAplicarMagia("Passo Nebuloso (conjuração)", 2);
                                            if (nivel >= 9)
                                            {
                                                com.TentaAplicarMagia("Ampliar Plantas (transmutação)", 3);
                                                com.TentaAplicarMagia("Proteção contra Energia (abjuração)", 3);
                                                if (nivel >= 13)
                                                {
                                                    com.TentaAplicarMagia("Tempestade de Gelo (evocação)", 4);
                                                    com.TentaAplicarMagia("Pele de Pedra (abjuração)", 4);
                                                    if (nivel >= 17)
                                                    {
                                                        com.TentaAplicarMagia("Comunhão com a Natureza (adivinhação)", 5);
                                                        com.TentaAplicarMagia("Caminhar em Árvores (conjuração)", 5);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 2:
                                        com.TentaAplicarMagia("Perdição (encantamento)", 1);
                                        com.TentaAplicarMagia("Marca do Caçador (adivinhação)", 1);
                                        if (nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Imobilizar Pessoa (encantamento)", 2);
                                            com.TentaAplicarMagia("Passo Nebuloso (conjuração)", 2);
                                            if (nivel >= 9)
                                            {
                                                com.TentaAplicarMagia("Velocidade (transmutação)", 3);
                                                com.TentaAplicarMagia("Proteção contra Energia (abjuração)", 3);
                                                if (nivel >= 13)
                                                {
                                                    com.TentaAplicarMagia("Banimento (abjuração)", 4);
                                                    com.TentaAplicarMagia("Porta Dimensional (conjuração)", 4);
                                                    if (nivel >= 17)
                                                    {
                                                        com.TentaAplicarMagia("Imobilizar Monstro (encantamento)", 5);
                                                        com.TentaAplicarMagia("Vidência (adivinhação)", 5);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                }
                            }
                            else if(codigo == "EDLu")
                            {
                                string[] estilos = { "Combate com Armas Grandes", "Defesa", "Duelismo", "Proteção" };
                                switch (estiloDeLuta)
                                {
                                    case 1:
                                        com.AdicionaHabilidadeClasse(estilos[0]);
                                        break;
                                    case 2:
                                        com.AdicionaHabilidadeClasse(estilos[1]);
                                        break;
                                    case 3:
                                        com.AdicionaHabilidadeClasse(estilos[2]);
                                        break;
                                    case 4:
                                        com.AdicionaHabilidadeClasse(estilos[3]);
                                        break;
                                    default:
                                        int rand = Random.Range(0, 4);
                                        com.AdicionaHabilidadeClasse(estilos[rand]);
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
        com.CanalizarDivindadePaladino(divindade);
    }
}
