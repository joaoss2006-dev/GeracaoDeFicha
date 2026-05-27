using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Clerigo : MonoBehaviour
{
    private Controller com;
    private Classes cla;
    void Start()
    {
        com = FindObjectOfType<Controller>();
        cla = FindObjectOfType<Classes>();
    }

    // Update is called once per frame

    private static readonly Dictionary<int, int[]> TabelaMagia = new()
    {
        { 1, new int[] { 3, 2, 0, 0, 0, 0, 0, 0, 0, 0 } },
        { 2, new int[] { 3, 3, 0, 0, 0, 0, 0, 0, 0, 0 } },
        { 3, new int[] { 3, 4, 2, 0, 0, 0, 0, 0, 0, 0 } },
        { 4, new int[] { 4, 4, 3, 0, 0, 0, 0, 0, 0, 0 } },
        { 5, new int[] { 4, 4, 3, 2, 0, 0, 0, 0, 0, 0 } },
        { 6, new int[] { 4, 4, 3, 3, 0, 0, 0, 0, 0, 0 } },
        { 7, new int[] { 4, 4, 3, 3, 1, 0, 0, 0, 0, 0 } },
        { 8, new int[] { 4, 4, 3, 3, 2, 0, 0, 0, 0, 0 } },
        { 9, new int[] { 4, 4, 3, 3, 3, 1, 0, 0, 0, 0 } },
        { 10, new int[] { 5, 4, 3, 3, 3, 2, 0, 0, 0, 0 } },
        { 11, new int[] { 5, 4, 3, 3, 3, 2, 1, 0, 0, 0 } },
        { 12, new int[] { 5, 4, 3, 3, 3, 2, 1, 0, 0, 0 } },
        { 13, new int[] { 5, 4, 3, 3, 3, 2, 1, 1, 0, 0 } },
        { 14, new int[] { 5, 4, 3, 3, 3, 2, 1, 1, 0, 0 } },
        { 15, new int[] { 5, 4, 3, 3, 3, 2, 1, 1, 1, 0 } },
        { 16, new int[] { 5, 4, 3, 3, 3, 2, 1, 1, 1, 0 } },
        { 17, new int[] { 5, 4, 3, 3, 3, 2, 1, 1, 1, 1 } },
        { 18, new int[] { 5, 4, 3, 3, 3, 3, 1, 1, 1, 1 } },
        { 19, new int[] { 5, 4, 3, 3, 3, 3, 2, 1, 1, 1 } },
        { 20, new int[] { 5, 4, 3, 3, 3, 3, 2, 2, 1, 1 } }
    };

    public void LendoArquivo()
    {
        int nivel = com.PassaNivel();
        string path = Path.Combine(Application.streamingAssetsPath, "Texts/Classes/Clérigo.txt");
        string[] linhas = File.ReadAllLines(path, System.Text.Encoding.UTF7);
        List<string> pericias = new List<string>() { };
        List<string> canalizarDivindade = new List<string>() { };
        int opcoes = -1;
        int pegarQuantos = 0;
        int esperarSub = 0;
        int contadorLinha = 0;
        int quantasLinhasSub = 0;
        bool noSub = false;
        int subClasse = -1;
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
                        if (subCl == -1) { subClasse = Random.Range(0, 7); }
                        else { subClasse = subCl; }
                        int comeco = int.Parse(linha.Substring(5 + 6 * subClasse, 2));
                        int final = int.Parse(linha.Substring(8 + 6 * subClasse, 2));
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
                                string path1 = Path.Combine(Application.streamingAssetsPath, "Texts\\Magias\\TruquesClérigo.txt");
                                string[] truquesClerigo = File.ReadAllLines(path1, System.Text.Encoding.UTF7);
                                for (int i = 0; i < tabela[0]; i++)
                                {
                                    int indice = Random.Range(0, truquesClerigo.Length);
                                    int contador = 0;
                                    while (com.EstaNosTruques(truquesClerigo[indice]))
                                    {
                                        indice = Random.Range(0, truquesClerigo.Length);
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
                                                    if (tabela[7] > 0)
                                                    {
                                                        nivelMagias++;
                                                        if (tabela[8] > 0)
                                                        {
                                                            nivelMagias++;
                                                            if (tabela[9] > 0) { nivelMagias++; }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                //pegando as magias
                                for (int i = 0; i < (com.PassaModificadorAtributo(4) + nivel); i++)
                                {
                                    int qualTabela = Random.Range(1, nivelMagias + 1);
                                    string path2 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\Clérigo{qualTabela}.txt");
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
                            else if(codigo == "CaDi")
                            {
                                string ca = linha.Substring(10);
                                canalizarDivindade.Add(ca);
                            }
                            else if(codigo == "TrAd")
                            {
                                com.EstaNosTruques("Luz (evocação)");
                            }
                            else if(codigo == "MaDo")
                            {
                                switch (subClasse)
                                {
                                    case 0:
                                        com.TentaAplicarMagia("Comando (encantamento)", 1);
                                        com.TentaAplicarMagia("Identificação (adivinhação)", 1);
                                        if (nivel >= 3)
                                        {
                                            com.TentaAplicarMagia("Augúrio (adivinhação)", 2);
                                            com.TentaAplicarMagia("Sugestão (encantamento)", 2);
                                            if(nivel >= 5)
                                            {
                                                com.TentaAplicarMagia("Dificultar Detecção (abjuração)", 3);
                                                com.TentaAplicarMagia("Falar com os Mortos (necromancia)", 3);
                                                if(nivel >= 7)
                                                {
                                                    com.TentaAplicarMagia("Olho Arcano (adivinhação)", 4);
                                                    com.TentaAplicarMagia("Confusão (encantamento)", 4);
                                                    if (nivel >= 9)
                                                    {
                                                        com.TentaAplicarMagia("Conhecimento Lendário (adivinhação)", 5);
                                                        com.TentaAplicarMagia("Vidência (adivinhação)", 5);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 1:
                                        com.TentaAplicarMagia("Enfeitiçar Pessoa (encantamento)", 1);
                                        com.TentaAplicarMagia("Disfarçar-se (ilusão)", 1);
                                        if (nivel >= 3)
                                        {
                                            com.TentaAplicarMagia("Reflexos (ilusão)", 2);
                                            com.TentaAplicarMagia("Passos sem Pegadas (abjuração)", 2);
                                            if (nivel >= 5)
                                            {
                                                com.TentaAplicarMagia("Piscar (transmutação)", 3);
                                                com.TentaAplicarMagia("Dissipar Magia (abjuração)", 3);
                                                if (nivel >= 7)
                                                {
                                                    com.TentaAplicarMagia("Porta Dimensional (conjuração)", 4);
                                                    com.TentaAplicarMagia("Metamorfose (transmutação)", 4);
                                                    if (nivel >= 9)
                                                    {
                                                        com.TentaAplicarMagia("Dominar Pessoa (encantamento)", 5);
                                                        com.TentaAplicarMagia("Modificar Memória (encantamento)", 5);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 2:
                                        //Colocando armadura pesada
                                        int rand2 = Random.Range(0, 4);
                                        if (rand2 == 0) { com.PegaClasseArmadura(14, false, true); }
                                        else if (rand2 == 1) { com.PegaClasseArmadura(16, false, true); }
                                        else if (rand2 == 2) { com.PegaClasseArmadura(17, false, true); }
                                        else if (rand2 == 3) { com.PegaClasseArmadura(18, false, true); }
                                        com.QualArmadura("Armadura Pesada");
                                        //-------------------------
                                        com.TentaAplicarMagia("Auxílio Divino (evocação)", 1);
                                        com.TentaAplicarMagia("Escudo da Fé (abjuração)", 1);
                                        if (nivel >= 3)
                                        {
                                            com.TentaAplicarMagia("Arma Mágica (transmutação)", 2);
                                            com.TentaAplicarMagia("Arma Espiritual (evocação)", 2);
                                            if (nivel >= 5)
                                            {
                                                com.TentaAplicarMagia("Manto do Cruzado (evocação)", 3);
                                                com.TentaAplicarMagia("Espíritos Guardiões (conjuração)", 3);
                                                if (nivel >= 7)
                                                {
                                                    com.TentaAplicarMagia("Movimentação Livre (abjuração)", 4);
                                                    com.TentaAplicarMagia("Pele de Pedra (abjuração)", 4);
                                                    if (nivel >= 9)
                                                    {
                                                        com.TentaAplicarMagia("Coluna de Chamas (evocação)", 5);
                                                        com.TentaAplicarMagia("Imobilizar Monstro (encantamento)", 5);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 3:
                                        com.TentaAplicarMagia("Mãos Flamejantes (evocação)", 1);
                                        com.TentaAplicarMagia("Fogo das Fadas (evocação)", 1);
                                        if (nivel >= 3)
                                        {
                                            com.TentaAplicarMagia("Esfera Flamejante (conjuração)", 2);
                                            com.TentaAplicarMagia("Raio Ardente (evocação)", 2);
                                            if (nivel >= 5)
                                            {
                                                com.TentaAplicarMagia("Luz do Dia (evocação)", 3);
                                                com.TentaAplicarMagia("Bola de Fogo (evocação)", 3);
                                                if (nivel >= 7)
                                                {
                                                    com.TentaAplicarMagia("Guardião da Fé (conjuração)", 4);
                                                    com.TentaAplicarMagia("Muralha de Fogo (evocação)", 4);
                                                    if (nivel >= 9)
                                                    {
                                                        com.TentaAplicarMagia("Coluna de Chamas (evocação)", 5);
                                                        com.TentaAplicarMagia("Vidência (adivinhação)", 5);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 4:
                                        //Colocando armadura pesada
                                        int rand3 = Random.Range(0, 4);
                                        if (rand3 == 0) { com.PegaClasseArmadura(14, false, true); }
                                        else if (rand3 == 1) { com.PegaClasseArmadura(16, false, true); }
                                        else if (rand3 == 2) { com.PegaClasseArmadura(17, false, true); }
                                        else if (rand3 == 3) { com.PegaClasseArmadura(18, false, true); }
                                        com.QualArmadura("Armadura Pesada");
                                        //-------------------------
                                        com.TentaAplicarMagia("Amizade Animal (encantamento)", 1);
                                        com.TentaAplicarMagia("Falar com Animais (adivinhação)", 1);
                                        if (nivel >= 3)
                                        {
                                            com.TentaAplicarMagia("Pele de Árvore (transmutação)", 2);
                                            com.TentaAplicarMagia("Crescer Espinhos (transmutação)", 2);
                                            if (nivel >= 5)
                                            {
                                                com.TentaAplicarMagia("Ampliar Plantas (transmutação)", 3);
                                                com.TentaAplicarMagia("Muralha de Vento (evocação)", 3);
                                                if (nivel >= 7)
                                                {
                                                    com.TentaAplicarMagia("Dominar Besta (encantamento)", 4);
                                                    com.TentaAplicarMagia("Vinha Esmagadora (conjuração)", 4);
                                                    if (nivel >= 9)
                                                    {
                                                        com.TentaAplicarMagia("Praga de Insetos (conjuração)", 5);
                                                        com.TentaAplicarMagia("Caminhar em Árvores (conjuração)", 5);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 5:
                                        //Colocando armadura pesada
                                        int rand4 = Random.Range(0, 4);
                                        if (rand4 == 0) { com.PegaClasseArmadura(14, false, true); }
                                        else if (rand4 == 1) { com.PegaClasseArmadura(16, false, true); }
                                        else if (rand4 == 2) { com.PegaClasseArmadura(17, false, true); }
                                        else if (rand4 == 3) { com.PegaClasseArmadura(18, false, true); }
                                        com.QualArmadura("Armadura Pesada");
                                        //-------------------------
                                        com.TentaAplicarMagia("Névoa Obscurecente (conjuração)", 1);
                                        com.TentaAplicarMagia("Onda Trovejante (evocação)", 1);
                                        if (nivel >= 3)
                                        {
                                            com.TentaAplicarMagia("Lufada de Vento (evocação)", 2);
                                            com.TentaAplicarMagia("Despedaçar (evocação)", 2);
                                            if (nivel >= 5)
                                            {
                                                com.TentaAplicarMagia("Convocar Relâmpagos (conjuração)", 3);
                                                com.TentaAplicarMagia("Nevasca (conjuração)", 3);
                                                if (nivel >= 7)
                                                {
                                                    com.TentaAplicarMagia("Controlar a Água (transmutação)", 4);
                                                    com.TentaAplicarMagia("Tempestade de Gelo (evocação)", 4);
                                                    if (nivel >= 9)
                                                    {
                                                        com.TentaAplicarMagia("Onda Destrutiva (evocação)", 5);
                                                        com.TentaAplicarMagia("Praga de Insetos (conjuração)", 5);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case 6:
                                        //Colocando armadura pesada
                                        int rand5 = Random.Range(0, 4);
                                        if (rand5 == 0) { com.PegaClasseArmadura(14, false, true); }
                                        else if (rand5 == 1) { com.PegaClasseArmadura(16, false, true); }
                                        else if (rand5 == 2) { com.PegaClasseArmadura(17, false, true); }
                                        else if (rand5 == 3) { com.PegaClasseArmadura(18, false, true); }
                                        com.QualArmadura("Armadura Pesada");
                                        //-------------------------
                                        com.TentaAplicarMagia("Bênção (encantamento)", 1);
                                        com.TentaAplicarMagia("Curar Ferimentos (evocação)", 1);
                                        if (nivel >= 3)
                                        {
                                            com.TentaAplicarMagia("Restauração Menor (abjuração)", 2);
                                            com.TentaAplicarMagia("Arma Espiritual (evocação)", 2);
                                            if (nivel >= 5)
                                            {
                                                com.TentaAplicarMagia("Sinal de Esperança (abjuração)", 3);
                                                com.TentaAplicarMagia("Revivificar (necromancia)", 3);
                                                if (nivel >= 7)
                                                {
                                                    com.TentaAplicarMagia("Proteção contra a Morte (abjuração)", 4);
                                                    com.TentaAplicarMagia("Guardião da Fé (conjuração)", 4);
                                                    if (nivel >= 9)
                                                    {
                                                        com.TentaAplicarMagia("Curar Ferimentos em Massa (evocação)", 5);
                                                        com.TentaAplicarMagia("Reviver os Mortos (necromancia)", 5);
                                                    }
                                                }
                                            }
                                        }
                                        break;
                                    case -1:
                                        Debug.LogError("ERRO");
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
        //aqui
        int quantos = 1;
        if(nivel >= 6)
        {
            quantos++;
            if(nivel >= 18)
            {
                quantos++;
            }
        }
        cla.PegaEspecificidade($"Usos de Canalizar Divindade: {quantos}");
        com.CanalizarDivindade(canalizarDivindade);
        
    }
}
