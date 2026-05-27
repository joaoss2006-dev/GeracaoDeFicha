using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Patrulheiro : MonoBehaviour
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
        { 2, new int[] {2, 2, 0, 0, 0, 0 } },
        { 3, new int[] {3, 3, 0, 0, 0, 0 } },
        { 4, new int[] {3, 3, 0, 0, 0, 0 } },
        { 5, new int[] {4, 4, 2, 0, 0, 0 } },
        { 6, new int[] {4, 4, 2, 0, 0, 0 } },
        { 7, new int[] {5, 4, 3, 0, 0, 0 } },
        { 8, new int[] {5, 4, 3, 0, 0, 0 } },
        { 9, new int[] {6, 4, 3, 2, 0, 0 } },
        { 10, new int[] {6, 4, 3, 2, 0, 0 } },
        { 11, new int[] {7, 4, 3, 3, 0, 0 } },
        { 12, new int[] {7, 4, 3, 3, 0, 0 } },
        { 13, new int[] {8, 4, 3, 3, 1, 0 } },
        { 14, new int[] {8, 4, 3, 3, 1, 0 } },
        { 15, new int[] {9, 4, 3, 3, 2, 0 } },
        { 16, new int[] {9, 4, 3, 3, 2, 0 } },
        { 17, new int[] {10, 4, 3, 3, 3, 1 } },
        { 18, new int[] {10, 4, 3, 3, 3, 1 } },
        { 19, new int[] {11, 4, 3, 3, 3, 2 } },
        { 20, new int[] {11, 4, 3, 3, 3, 2 } }
    };

    public void LendoArquivo(int estiloDeLuta)
    {
        int nivel = com.PassaNivel();
        string path = Path.Combine(Application.streamingAssetsPath, "Texts\\Classes\\Patrulheiro.txt");
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
                        //Caso haja nivel para adiquirir a habilidade
                        if (nivelRequisito <= nivel)
                        {
                            //Verificando se é algum código para algo especifico
                            string codigo = linha.Substring(5, 4);
                            if (codigo == "RASU")
                            {
                                com.VerificaHabiidadeRaca("Visão no Escuro");
                                if(nivel >= 3)
                                {
                                    com.TentaAplicarMagia("Disfarçar-se", 1);
                                    if(nivel >= 5)
                                    {
                                        com.TentaAplicarMagia("Truque de Corda", 2);
                                        if (nivel >= 9)
                                        {
                                            com.TentaAplicarMagia("Glifo de Vigilância", 3);
                                            if (nivel >= 13)
                                            {
                                                com.TentaAplicarMagia("Invisibilidade Maior", 4);
                                                if (nivel >= 17)
                                                {
                                                    com.TentaAplicarMagia("Similaridade", 5);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            else if (codigo == "MAGI")
                            {
                                com.AplicaCDMagias(4);
                                int[] tabela = TabelaMagia[nivel];

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
                                            if (tabela[5] > 0) { nivelMagias++; }
                                        }
                                    }
                                }


                                //pegando as magias
                                for (int i = 0; i < tabela[0]; i++)
                                {
                                    int qualTabela = Random.Range(1, nivelMagias + 1);
                                    string path1 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\Patrulheiro{qualTabela}.txt");
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
                                com.ColocaEspacosMagia(tabela[1], 1);
                                com.ColocaEspacosMagia(tabela[2], 2);
                                com.ColocaEspacosMagia(tabela[3], 3);
                                com.ColocaEspacosMagia(tabela[4], 4);
                                com.ColocaEspacosMagia(tabela[5], 5);
                            }
                            //Caso não seja código para nada, só adiciona a habilidade
                            else
                            {
                                if(linha.Substring(5) == "Presa do Caçador")
                                {
                                    string texto = "";
                                    int random = Random.Range(0, 3);
                                    switch (random)
                                    {
                                        case 0:
                                            texto += "Assassinho de Colossos:\n";
                                            texto += "Sua tenacidade pode derrubar os mais poderosos oponentes. Quando você atinge uma criatura com um ataque com arma, a criatura sofre 1d8 de dano extra, se ela estiver abaixo do máximo de pontos de vida dela. Você só pode causar esse dano extra uma vez por turno.";
                                            break;
                                        case 1:
                                            texto += "Matador de Gigantes:\n";
                                            texto += "Quando uma criatura Grande ou maior a até 1,5 metro de você atingir ou errar um ataque contra você, você pode usar sua reação para atacar a criatura, imediatamente após o ataque dela, considerando que você possa ver a criatura.";
                                            break;
                                        case 2:
                                            texto += "Destruidor de Hordas:\n";
                                            texto += "Uma vez em cada um dos seus turnos, quando você fizer um ataque com arma, você pode realizar outro ataque com a mesma arma contra uma criatura diferente que esteja a até 1,5 metro do alvo original e esteja no alcance da sua arma.";
                                            break;
                                    }
                                    com.AdicionaHabilidadeDireto(texto);
                                }
                                else if(linha.Substring(5) == "Táticas Defensivas")
                                {
                                    string texto = "";
                                    int random = Random.Range(0, 3);
                                    switch (random)
                                    {
                                        case 0:
                                            texto += "Escapar da Horda:\n";
                                            texto += "Ataques de oportunidade contra você são feitos com desvantagem.";
                                            break;
                                        case 1:
                                            texto += "Defesa Contra Múltiplos Ataques:\n";
                                            texto += "Quando uma criatura atinge você com um ataque, você recebe +4 de bônus na CA contra todos os ataques subsequentes feitos por essa criatura no resto do turno.";
                                            break;
                                        case 2:
                                            texto += "Vontade de Aço:\n";
                                            texto += "Você tem vantagem em testes de resistência para evitar ser amedrontado.";
                                            break;
                                    }
                                    com.AdicionaHabilidadeDireto(texto);
                                }
                                else if(linha.Substring(5) == "Ataque Multiplo")
                                {
                                    string texto = "";
                                    int random = Random.Range(0, 2);
                                    switch (random)
                                    {
                                        case 0:
                                            texto += "Saraivada:\n";
                                            texto += "Você pode usar sua ação para realizar um ataque à distância contra qualquer número de criatura a até 3 metros de um ponto que você possa ver, no alcance da sua arma. Você deve ter munição para cada alvo, como normal, e você realiza uma jogada de ataque separada para cada alvo.";
                                            break;
                                        case 1:
                                            texto += "Ataque Giratório:\n";
                                            texto += "Você pode usar sua ação para realizar um ataque corpo-a-corpo contra qualquer número de criaturas a até 1,5 metro de você, realizando uma jogada de ataque separada para cada alvo.";
                                            break;
                                    }
                                    com.AdicionaHabilidadeDireto(texto);
                                }
                                else if(linha.Substring(5) == "Defesa de Caçador Superior")
                                {
                                    string texto = "";
                                    int random = Random.Range(0, 3);
                                    switch (random)
                                    {
                                        case 0:
                                            texto += "Evasão:\n";
                                            texto += "Você pode esquivar-se agilmente de certos efeitos em área, como o sopro de fogo de um dragão vermelho ou uma magia relâmpago. Quando você for alvo de um efeito que exige um teste de resistência de Destreza para sofrer metade do dano, você não sofre dano algum se passar, e somente metade do dano se falhar.";
                                            break;
                                        case 1:
                                            texto += "Manter-se Contra a Maré:\n";
                                            texto += "Quando uma criatura hostil errar você com um ataque corpo-a-corpo, você pode usar sua reação para forçar a criatura a repetir o mesmo ataque contra outra criatura (que não ela mesma), à sua escolha.";
                                            break;
                                        case 2:
                                            texto += "Esquiva Sobrenaturals:\n";
                                            texto += "Quando um atacante que você possa ver, atinge você com um ataque, você pode usar sua reação para reduzir o dano causado pelo ataque à metade.";
                                            break;
                                    }

                                    com.AdicionaHabilidadeDireto(texto);
                                }
                                else
                                {
                                    com.AdicionaHabilidadeClasse(linha.Substring(5));
                                }
                            }
                        }
                        break;
                }
            }

        }
        //Adicionando o estilo certo de estilo de combate
        if(nivel >= 2)
        {
            string[] estilos = { "Arquearia", "Combate com Duas Armas", "Defesa", "Duelismo" };
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
    }
}
