using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Monge : MonoBehaviour
{
    private Controller com;
    private Classes cla;
    void Start()
    {
        com = FindObjectOfType<Controller>();
        cla = FindObjectOfType<Classes>();
    }


    public void LendoArquivo()
    {
        int nivel = com.PassaNivel();
        string path = Path.Combine(Application.streamingAssetsPath, "Texts\\Classes\\Monge.txt");
        string[] linhas = File.ReadAllLines(path, System.Text.Encoding.UTF7);
        List<string> pericias = new List<string>() { };
        int opcoes = -1;
        int pegarQuantos = 0;
        int esperarSub = 0;
        int contadorLinha = 0;
        int quantasLinhasSub = 0;
        bool noSub = false;
        List<string> elementos = new List<string>() { };

        elementos.Add("Chicote de Água");
        elementos.Add("Golpe de Varredura Cauterizante");
        elementos.Add("Investida dos Espíritos da Ventania");
        elementos.Add("Moldar o Rio Corrente");
        elementos.Add("Presas da Serpente de Fogo");
        elementos.Add("Punho do Ar Continuo");
        elementos.Add("Punho dos Quatro Trovões");
        if(nivel >= 6)
        {
            elementos.Add("Gongo do Pico");
            elementos.Add("Serragem do Vento do Norte");
            if(nivel >= 11)
            {
                elementos.Add("Cavalgar o Vento");
                elementos.Add("Chamas da Fénix");
                elementos.Add("Postura da Neblina");
                if(nivel >= 17)
                {
                    elementos.Add("Defesa Eterna da Montanha");
                    elementos.Add("Onda de Pedras Rolantes");
                    elementos.Add("Rio de Chamas Famintas");
                    elementos.Add("Sopro do Inverno");
                }
            }
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
                            if (codigo == "DSAr")
                            {
                                com.PegaClasseArmadura(10 + com.PassaModificadorAtributo(1) + com.PassaModificadorAtributo(4), false, true);
                            }
                            else if(codigo == "MSAr")
                            {
                                int deslocamentoAdicional = 2;
                                if(nivel >= 6)
                                {
                                    deslocamentoAdicional += 1;
                                    if (nivel >= 10)
                                    {
                                        deslocamentoAdicional += 1;
                                        if(nivel >= 14)
                                        {
                                            deslocamentoAdicional += 1;
                                            if(nivel >= 18)
                                            {
                                                deslocamentoAdicional += 1;
                                            }
                                        }
                                    }
                                }
                                com.AdicionaDeslocamento(deslocamentoAdicional);
                            }
                            else if(codigo == "Elem")
                            {
                                //Lembrar de adicionar o "truque base"
                                com.AdicionaHabilidadeClasse("Sintonia Elemental");
                                int quantos = 2;
                                if(nivel >= 6)
                                {
                                    quantos++;
                                    if(nivel >= 11)
                                    {
                                        quantos++;
                                        if (nivel >= 17)
                                        {
                                            quantos++;
                                        }
                                    }
                                }
                                for(int i = 0; i < quantos; i++)
                                {
                                    int rand = Random.Range(0, elementos.Count);
                                    com.AdicionaHabilidadeClasse(elementos[rand]);
                                    elementos.Remove(elementos[rand]);
                                }

                            }
                            else
                            {
                                com.AdicionaHabilidadeClasse(linha.Substring(5));
                            }
                        }
                        break;
                }
            }

        }
        //Aqui
        string text = "Dado de Artes Marciais: 1d";
        int dado = 4;
        if (nivel >= 5)
        {
            dado += 2;
            if (nivel >= 11)
            {
                dado += 2;
                if (nivel >= 17)
                {
                    dado += 2;
                }
            }
        }
        text += dado.ToString();
        if(nivel >= 2)
        {
            text += "   //   ";
            text += $"Pontos de Chi: {nivel}\nCD habilidades de Chi: 8 + prof + Sab";
        }
        cla.PegaEspecificidade(text);
    }
}

