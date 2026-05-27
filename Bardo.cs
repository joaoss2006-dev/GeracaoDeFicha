using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Bardo : MonoBehaviour
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
        { 1, new int[] { 2, 4, 2, 0, 0, 0, 0, 0, 0, 0, 0} },
        { 2, new int[] { 2, 5, 3, 0, 0, 0, 0, 0, 0, 0, 0 } },
        { 3, new int[] { 2, 6, 4, 2, 0, 0, 0, 0, 0, 0, 0 } },
        { 4, new int[] { 3, 7, 4, 3, 0, 0, 0, 0, 0, 0, 0 } },
        { 5, new int[] { 3, 8, 4, 3, 2, 0, 0, 0, 0, 0, 0 } },
        { 6, new int[] { 3, 9, 4, 3, 3, 0, 0, 0, 0, 0, 0 } },
        { 7, new int[] { 3, 10, 4, 3, 3, 1, 0, 0, 0, 0, 0 } },
        { 8, new int[] { 3, 11, 4, 3, 3, 2, 0, 0, 0, 0, 0 } },
        { 9, new int[] { 3, 12, 4, 3, 3, 3, 1, 0, 0, 0, 0 } },
        { 10, new int[] { 4, 14, 4, 3, 3, 3, 2, 0, 0, 0, 0 } },
        { 11, new int[] { 4, 15, 4, 3, 3, 3, 2, 1, 0, 0, 0 } },
        { 12, new int[] { 4, 15, 4, 3, 3, 3, 2, 1, 0, 0, 0 } },
        { 13, new int[] { 4, 16, 4, 3, 3, 3, 2, 1, 1, 0, 0 } },
        { 14, new int[] { 4, 18, 4, 3, 3, 3, 2, 1, 1, 0, 0 } },
        { 15, new int[] { 4, 19, 4, 3, 3, 3, 2, 1, 1, 1, 0 } },
        { 16, new int[] { 4, 19, 4, 3, 3, 3, 2, 1, 1, 1, 0 } },
        { 17, new int[] { 4, 20, 4, 3, 3, 3, 2, 1, 1, 1, 1 } },
        { 18, new int[] { 4, 22, 4, 3, 3, 3, 3, 1, 1, 1, 1 } },
        { 19, new int[] { 4, 22, 4, 3, 3, 3, 3, 2, 1, 1, 1 } },
        { 20, new int[] { 4, 22, 4, 3, 3, 3, 3, 2, 2, 1, 1 } }
    };

    public void LendoArquivo()
    {
        int nivel = com.PassaNivel();
        string path = Path.Combine(Application.streamingAssetsPath, "Texts/Classes/Bardo.txt");
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
                        if (nivelRequisito <= nivel)
                        {

                            //Caso haja nivel para adiquirir a habilidade
                            //Verificando se é algum código para algo especifico
                            string codigo = linha.Substring(5, 4);
                            if (codigo == "MAGI")
                            {
                                com.AplicaCDMagias(5);
                                int[] tabela = TabelaMagia[nivel];
                                //Pegando os Truques da tabela

                                for (int i = 0; i < tabela[0]; i++)
                                {
                                    string path2 = Path.Combine(Application.streamingAssetsPath, "Texts/Magias/TruquesBardo.txt");
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
                                int quantas = tabela[1];
                                if (nivel >= 10)
                                {
                                    quantas -= 2;
                                    if(nivel >= 14)
                                    {
                                        quantas -= 2;
                                        if (nivel >= 18)
                                        {
                                            quantas -= 2;
                                        }
                                    }
                                }
                                for (int i = 0; i < quantas; i++)
                                {
                                    int qualTabela = Random.Range(1, nivelMagias + 1);
                                    string path1 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\Bardo{qualTabela}.txt");
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
                            else if(codigo == "PrAd")
                            {
                                string[] per = {"Lidar com Animais", "Acrobacia", "Atletismo", "Atuação",  "Enganação",  "Furtividade", "História", "Intimidação", "Intuição", "Investigação", "Medicina", "Natureza", "Percepção", "Persuasão", "Prestidigitação", "Religião", "Sobrevivência" };
                                int rand = Random.Range(0, per.Length);
                                int contador = 0;
                                while (com.TentaAplicarPericia(per[rand]) == 0)
                                {
                                    rand = Random.Range(0, per.Length);
                                    if(contador > 20)
                                    {
                                        break;
                                    }
                                    contador++;
                                }
                            }
                            else if(codigo == "SeMa")
                            {
                                int quantas = 2;
                                if(nivel >= 14)
                                {
                                    quantas += 2;
                                    if (nivel >= 18)
                                    {
                                        quantas += 2;
                                    }
                                }
                                for (int i = 0; i < quantas; i++)
                                {
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    string[] classesMagicas = { "Bardo", "Feiticeiro", "Mago", "Patrulheiro" };
                                    int modClasse = Random.Range(0, classesMagicas.Length);
                                    int[] tabela = TabelaMagia[nivel];
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
                                    int qualTabela = Random.Range(0, nivelMagias + 1);
                                    if (classesMagicas[modClasse] == "Patrulheiro" && qualTabela > 5)
                                    {
                                        qualTabela = 5;
                                    }
                                    if(qualTabela == 0)
                                    { 
                                        if(classesMagicas[modClasse] == "Patrulheiro")
                                        {
                                            string path3 = Path.Combine(Application.streamingAssetsPath, $"Texts/Magias/{classesMagicas[modClasse]}1.txt");
                                            string[] magias = File.ReadAllLines(path3, System.Text.Encoding.UTF7);
                                            int rand = Random.Range(0, magias.Length);
                                            //garantindo que não vá ficar preso no while
                                            int contador = 0;
                                            while (com.EstaNosTruques(magias[rand]))
                                            {
                                                rand = Random.Range(0, magias.Length);
                                                if (contador > 20)
                                                {
                                                    break;
                                                }
                                                contador++;
                                            }
                                        }
                                        else
                                        {
                                            string path4 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\Truques{classesMagicas[modClasse]}.txt");
                                            string[] magias = File.ReadAllLines(path4, System.Text.Encoding.UTF7);
                                            int rand = Random.Range(0, magias.Length);
                                            //garantindo que não vá ficar preso no while
                                            int contador = 0;
                                            while (com.EstaNosTruques(magias[rand]))
                                            {
                                                rand = Random.Range(0, magias.Length);
                                                if (contador > 20)
                                                {
                                                    break;
                                                }
                                                contador++;
                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        string path5 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\{classesMagicas[modClasse]}{qualTabela}.txt");
                                        string[] magias = File.ReadAllLines(path5, System.Text.Encoding.UTF7);
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
                                    
                                }
                            }
                            else if(codigo == "SMAd")
                            {
                                for(int i = 0; i < 2; i++)
                                {
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    //Resolver dps
                                    string[] classesMagicas = { "Bardo", "Feiticeiro", "Mago", "Patrulheiro" };
                                    int modClasse = Random.Range(0, classesMagicas.Length);
                                    int[] tabela = TabelaMagia[nivel];
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
                                    int qualTabela = Random.Range(0, nivelMagias + 1);
                                    if (classesMagicas[modClasse] == "Patrulheiro" && qualTabela > 5)
                                    {
                                        qualTabela = 5;
                                    }
                                    if(classesMagicas[modClasse] == "Patrulheiro" && qualTabela == 0)
                                    {
                                        qualTabela = 1;
                                    }
                                    if (qualTabela == 0)
                                    {
                                        string path6 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\Truques{classesMagicas[modClasse]}.txt");
                                        string[] magias = File.ReadAllLines(path6, System.Text.Encoding.UTF7);
                                        int rand = Random.Range(0, magias.Length);
                                        //garantindo que não vá ficar preso no while
                                        int contador = 0;
                                        while (com.EstaNosTruques(magias[rand]))
                                        {
                                            rand = Random.Range(0, magias.Length);
                                            if (contador > 20)
                                            {
                                                break;
                                            }
                                            contador++;
                                        }
                                    }
                                    else
                                    {
                                        string path7 = Path.Combine(Application.streamingAssetsPath, $"Texts\\Magias\\{classesMagicas[modClasse]}{qualTabela}.txt");
                                        string[] magias = File.ReadAllLines(path7, System.Text.Encoding.UTF7);
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
                                }
                                
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
        int dado = 6;
        int quantos = com.PassaModificadorAtributo(5);
        if(nivel >= 5)
        {
            dado += 2;
            if(nivel >= 10)
            {
                dado += 2;
                if(nivel >= 15) { dado += 2; }
            }
        }
        cla.PegaEspecificidade($"{quantos}d{dado} dados de inspiração");

    }
}
