using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarkovModelTrain
{
    class Program
    {
        static void Main(string[] args)
        {
            int nGestures=10;
            int nSamples= 55;

            List <List <int>> digits= new List<List<int>> ();
            int testIndexStandart= 46;

            #region TRAINDATA

            logHMM [] samples= new logHMM[nGestures];

            for(int i=0; i<nGestures; i++)
            {
                samples[i]= new logHMM(6,9);

                Console.WriteLine("Training digit" + (i+1) + ".......");

                for(int j=1; j<=nSamples; j++)
                {
                    try{
                        string fileName="Digit_" + (i+1) + "_" + j + ".txt";

                        using (StreamReader sr = new StreamReader(fileName))
                        {
                            String line = sr.ReadToEnd();
                            
                            int [] digit = StringToArray(line);

                            for(int iteration=0; iteration< 100; iteration++)
                            {
                                samples[i].baumWelchTrain(digit);


                            }
                        }

                        catch (Exception e)
                        {
                            Console.WriteLine("ERrror ");
                            Console.WriteLine(digit.Sum().ToString());
                            Condole.WriteLine("Digit 0: " + digits[13].ToString());

                        }
                    }

                    Console.WriteLine("----------------");
                }

                #endregion

                #region TESTDATA

                double probability = double.NegativeInfinity;
                int numberIS= 0;
                int trueDetected= 0;
                int totalSamples= 0;

                for(int digit =1; digit <= nGestures; digit++)
                {
                    int deneme123= 0;

                    for(int sample=1; sample <=nSamples; sample++)
                    {
                        try
                        {
                            string denemeFile= "Digit_" + digit + "_" + sample + ".txt";

                            using (StreamReader st= new StreamReader(denemeFile))
                            {
                                String line = sr.ReadToEnd();

                                int[] testData= StringToIntArray(line);

                                probability= double.NegativeInfinity;

                                numberIS=0;


                                for(int i=0; i< nGestures; i++)
                                {
                                    double temp= samples[i].prob_Obs_Seq(testData);

                                    Console.WriteLine("prob" + i + "is " + temp);

                                    if(temp > probability)
                                    {
                                        probability =temp;
                                        numberIS =i+1;

                                    }
                                }
                            }
                        }


                        catch(Exception e)
                        {
                            Console.WriteLine("ERROR 2");
                            Console.WriteLine(e.Message);


                        }

                        if(numberIS ==digit)
                        {
                            ++ trueDetected, ++ deneme123;
                        }

                        ++totalSamples;

                    }

                    Console.WriteLine("Digit: " + digit + " | True:" + deneme123 + " | False" + (nSamples - deneme123));
                }

                Console.WriteLine("-------\nRESULTS: \n");
                Console.WriteLine("true detected: " + trueDetected + "--> total samples: " + totalSamples );

                #endregion

                Console.ReadLine();


            }

            private static int[] StringToIntArray(string myNumbers)
            {
                List<int> myIntegers =new List<int>();
                Array.ForEach(myNumbers.Split(",".ToCharArray()), s =>
                {
                    int currentInt;
                    if(Int32.TryParse(s, out currentInt))
                        myIntegers.Add(currentInt);
                });

                return myIntegers.ToArray();
                
            }
        }
    }
}