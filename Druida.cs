using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Druida : MonoBehaviour
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
        { 1, new int[] { 2, 2, 0, 0, 0, 0, 0, 0, 0, 0 } },
        { 2, new int[] { 2, 3, 0, 0, 0, 0, 0, 0, 0, 0 } },
        { 3, new int[] { 2, 4, 2, 0, 0, 0, 0, 0, 0, 0 } },
        { 4, new int[] { 3, 4, 3, 0, 0, 0, 0, 0, 0, 0 } },
        { 5, new int[] { 3, 4, 3, 2, 0, 0, 0, 0, 0, 0 } },
        { 6, new int[] { 3, 4, 3, 3, 0, 0, 0, 0, 0, 0 } },
        { 7, new int[] { 3, 4, 3, 3, 1, 0, 0, 0, 0, 0 } },
        { 8, new int[] { 3, 4, 3, 3, 2, 0, 0, 0, 0, 0 } },
        { 9, new int[] { 3, 4, 3, 3, 3, 1, 0, 0, 0, 0 } },
        { 10, new int[] { 4, 4, 3, 3, 3, 2, 0, 0, 0, 0 } },
        { 11, new int[] { 4, 4, 3, 3, 3, 2, 1, 0, 0, 0 } },
        { 12, new int[] { 4, 4, 3, 3, 3, 2, 1, 0, 0, 0 } },
        { 13, new int[] { 4, 4, 3, 3, 3, 2, 1, 1, 0, 0 } },
        { 14, new int[] { 4, 4, 3, 3, 3, 2, 1, 1, 0, 0 } },
        { 15, new int[] { 4, 4, 3, 3, 3, 2, 1, 1, 1, 0 } },
        { 16, new int[] { 4, 4, 3, 3, 3, 2, 1, 1, 1, 0 } },
        { 17, new int[] { 4, 4, 3, 3, 3, 2, 1, 1, 1, 1 } },
        { 18, new int[] { 4, 4, 3, 3, 3, 3, 1, 1, 1, 1 } },
        { 19, new int[] { 4, 4, 3, 3, 3, 3, 2, 1, 1, 1 } },
        { 20, new int[] { 4, 4, 3, 3, 3, 3, 2, 2, 1, 1 } }
    };

    public void LendoArquivo(int circuloTerra)
    {
        int nivel = com.PassaNivel();
        string path = Path.Combine(Application.streamingAssetsPath, "Texts/Classes/Druida.txt");
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
                                com.AplicaCDMagias(4);
                                //===========================================================================================================================================================
                                int[] tabela = TabelaMagia[nivel];
                                //Pegando os Truques da tabela
                                for (int i = 0; i < tabela[0]; i++)
                                {
                                    string path1 = Path.Combine(Application.streamingAssetsPath, "Texts/Magias/TruquesDruida.txt");
                                    string[] truquesDruida = File.ReadAllLines(path1, System.Text.Encoding.UTF7);
                                    int indice = Random.Range(0, truquesDruida.Length);
                                    int contador = 0;
                                    while (com.EstaNosTruques(truquesDruida[indice]))
                                    {
                                        indice = Random.Range(0, truquesDruida.Length);
                                        if (contador >= 20)
                                        {
                                            break;
                                        }
                                    }
                                }
                                //Verificando até que nível posso pegar as magias
                                int nivelMagias = 1;
                                if (tabela[2] > 0)
                                {
                                    nivelMagias++;
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
                                                    if(tabela[7] > 0)
                                                    {
                                                        nivelMagias++;
                                                        if (tabela[8] > 0)
                                                        {
                                                            nivelMagias++;
                                                            if (tabela[9] > 0)
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
                                for (int i = 0; i < (com.PassaModificadorAtributo(4)+nivel); i++)
                                {
                                    int qualTabela = Random.Range(1, nivelMagias + 1);
                                    string path2 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\Druida{qualTabela}.txt");
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
                                for(int i = 1; i <= 9; i++)
                                {
                                    com.ColocaEspacosMagia(tabela[i], i);
                                }
                            }
                            else if(codigo == "TrAd")
                            {
                                string path3 = Path.Combine(Application.streamingAssetsPath, "Texts\\Magias\\TruquesDruida.txt");
                                string[] truquesDruida = File.ReadAllLines(path3, System.Text.Encoding.UTF7);
                                int indice = Random.Range(0, truquesDruida.Length);
                                int contador = 0;
                                while (com.EstaNosTruques(truquesDruida[indice]))
                                {
                                    indice = Random.Range(0, truquesDruida.Length);
                                    if (contador >= 20)
                                    {
                                        break;
                                    }
                                }

                            }
                            else if(codigo == "MaCi")
                            {
                                int origem = circuloTerra;
                                if(origem == -1) { origem = Random.Range(0, 8); }
                                switch (origem)
                                {
                                    case 0:
                                        com.TentaAplicarMagia("Imobilizar Pessoa (encantamento)", 2);
                                        com.TentaAplicarMagia("Crescer Espinhos (transmutação)", 2);
                                        if(nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Nevasca (conjuração)", 3);
                                            com.TentaAplicarMagia("Lentidão (transmutação)", 3);
                                            if (nivel >= 7)
                                            {
                                                com.TentaAplicarMagia("Movimentação Livre (abjuração)", 4);
                                                com.TentaAplicarMagia("Tempestade de Gelo (evocação)", 4);
                                                if (nivel >= 9)
                                                {
                                                    com.TentaAplicarMagia("Comunhão com a Natureza (adivinhação)", 5);
                                                    com.TentaAplicarMagia("Cone de Frio (evocação)", 5);
                                                }
                                            }
                                        }
                                        break;
                                    case 1:
                                        com.TentaAplicarMagia("Passo Nebuloso (conjuração)", 2);
                                        com.TentaAplicarMagia("Reflexos (ilusão)", 2);
                                        if (nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Andar na Água (transmutação)", 3);
                                            com.TentaAplicarMagia("Respirar na Água (transmutação)", 3);
                                            if (nivel >= 7)
                                            {
                                                com.TentaAplicarMagia("Movimentação Livre (abjuração)", 4);
                                                com.TentaAplicarMagia("Controlar a Água (transmutação)", 4);
                                                if (nivel >= 9)
                                                {
                                                    com.TentaAplicarMagia("Vidência (adivinhação)", 5);
                                                    com.TentaAplicarMagia("Conjurar Elemental(conjuração)", 5);
                                                }
                                            }
                                        }
                                        break;
                                    case 2:
                                        com.TentaAplicarMagia("Nublar (ilusão)", 2);
                                        com.TentaAplicarMagia("Silêncio (ilusão)", 2);
                                        if (nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Criar Alimentos (conjuração)", 3);
                                            com.TentaAplicarMagia("Proteção contra Energia (abjuração)", 3);
                                            if (nivel >= 7)
                                            {
                                                com.TentaAplicarMagia("Praga (necromancia)", 4);
                                                com.TentaAplicarMagia("Terreno Alucinógeno (ilusão)", 4);
                                                if (nivel >= 9)
                                                {
                                                    com.TentaAplicarMagia("Muralha de Pedra (evocação)", 5);
                                                    com.TentaAplicarMagia("Praga de Insetos (conjuração)", 5);
                                                }
                                            }
                                        }
                                        break;
                                    case 3:
                                        com.TentaAplicarMagia("Patas de Aranha (transmutação)", 2);
                                        com.TentaAplicarMagia("Pele de Árvore (transmutação)", 2);
                                        if (nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Convocar Relâmpagos (conjuração)", 3);
                                            com.TentaAplicarMagia("Ampliar Plantas (transmutação)", 3);
                                            if (nivel >= 7)
                                            {
                                                com.TentaAplicarMagia("Adivinhação (adivinhação)", 4);
                                                com.TentaAplicarMagia("Movimentação Livre (abjuração)", 4);
                                                if (nivel >= 9)
                                                {
                                                    com.TentaAplicarMagia("Comunhão com a Natureza (adivinhação)", 5);
                                                    com.TentaAplicarMagia("Caminhar em Árvores (conjuração)", 5);
                                                }
                                            }
                                        }
                                        break;
                                    case 4:
                                        com.TentaAplicarMagia("Crescer Espinhos (transmutação)", 2);
                                        com.TentaAplicarMagia("Patas de Aranha (transmutação)", 2);
                                        if (nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Mesclar-se às Rochas (transmutação)", 3);
                                            com.TentaAplicarMagia("Relâmpago (evocação)", 3);
                                            if (nivel >= 7)
                                            {
                                                com.TentaAplicarMagia("Moldar Rochas (transmutação)", 4);
                                                com.TentaAplicarMagia("Pele de Pedra (abjuração)", 4);
                                                if (nivel >= 9)
                                                {
                                                    com.TentaAplicarMagia("Criar Passagem (transmutação)", 5);
                                                    com.TentaAplicarMagia("Muralha de Pedra (evocação)", 5);
                                                }
                                            }
                                        }
                                        break;
                                    case 5:
                                        com.TentaAplicarMagia("Escuridão (transmutação)", 2);
                                        com.TentaAplicarMagia("Flecha Ácida de Melf (evocação)", 2);
                                        if (nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Andar na Água (transmutação)", 3);
                                            com.TentaAplicarMagia("Névoa Fétida (conjuração)", 3);
                                            if (nivel >= 7)
                                            {
                                                com.TentaAplicarMagia("Localizar Criatura (adivinhação)", 4);
                                                com.TentaAplicarMagia("Movimentação Livre (abjuração)", 4);
                                                if (nivel >= 9)
                                                {
                                                    com.TentaAplicarMagia("Praga de Insetos (conjuração)", 5);
                                                    com.TentaAplicarMagia("Vidência (adivinhação)", 5);
                                                }
                                            }
                                        }
                                        break;
                                    case 6:
                                        com.TentaAplicarMagia("Invisibilidade (ilusão)", 2);
                                        com.TentaAplicarMagia("Passos sem Pegadas (abjuração)", 2);
                                        if (nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Luz do Dia (evocação)", 3);
                                            com.TentaAplicarMagia("Velocidade (transmutação)", 3);
                                            if (nivel >= 7)
                                            {
                                                com.TentaAplicarMagia("Adivinhação (adivinhação)", 4);
                                                com.TentaAplicarMagia("Movimentação Livre (abjuração)", 4);
                                                if (nivel >= 9)
                                                {
                                                    com.TentaAplicarMagia("Praga de Insetos (conjuração)", 5);
                                                    com.TentaAplicarMagia("Sonho (ilusão)", 5);
                                                }
                                            }
                                        }
                                        break;
                                    case 7:
                                        com.TentaAplicarMagia("Patas de Aranha (transmutação)", 2);
                                        com.TentaAplicarMagia("Teia (conjuração)", 2);
                                        if (nivel >= 5)
                                        {
                                            com.TentaAplicarMagia("Forma Gasosa (transmutação)", 3);
                                            com.TentaAplicarMagia("Névoa Fétida (conjuração)", 3);
                                            if (nivel >= 7)
                                            {
                                                com.TentaAplicarMagia("Invisibilidade Maior (ilusão)", 4);
                                                com.TentaAplicarMagia("Moldar Rochas (transmutação)", 4);
                                                if (nivel >= 9)
                                                {
                                                    com.TentaAplicarMagia("Praga de Insetos (conjuração)", 5);
                                                    com.TentaAplicarMagia("Névoa Mortal (conjuração)", 5);
                                                }
                                            }
                                        }
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
        int quantasFormas = 0;
        if(nivel >= 2)
        {
            quantasFormas = 2;
        }
        if(nivel != 20)
        {
            cla.PegaEspecificidade($"Cargas de Forma Selvagem: {quantasFormas}");
        }
        else
        {
            cla.PegaEspecificidade("Cargas de Forma Selvagem: ilimitado");
        }
        //aqui
    }
}
