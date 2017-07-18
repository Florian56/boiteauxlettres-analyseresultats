using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AnalyseResultatsAucunCourrier
{
    class Program
    {
        static void Main(string[] args)
        {
            var fichierSource = @"C:\Users\Florian\Downloads\MYSController\Logs\AvecCourrier_Complet.log";
            var fichierCible = @"C:\Users\Florian\Downloads\MYSController\Logs\Resultats.log";
            var patternRecherche = "2;0;1;0;23;";
            
            var ligneLue = string.Empty;
            var dateHeurePrecedente = string.Empty;

            var valeursInstantaneesDesSixCapteurs = new List<int>();
            var valeursPremierCapteur = new List<int>();
            var valeursDeuxiemeCapteur = new List<int>();
            var valeursTroisiemeCapteur = new List<int>();
            var valeursQuatriemeCapteur = new List<int>();
            var valeursCinquiemeCapteur = new List<int>();
            var valeursSixiemeCapteur = new List<int>();

            var fluxFichierSource = new StreamReader(fichierSource);
            while ((ligneLue = fluxFichierSource.ReadLine()) != null)
            {
                if (ligneLue.Contains(patternRecherche))
                {
                    var dateHeureCourante = ligneLue.Substring(2, 16);
                    var nombreCourant = ligneLue.Substring(46, ligneLue.Length - 46);

                    if (dateHeureCourante != dateHeurePrecedente)
                    {
                        valeursInstantaneesDesSixCapteurs.Clear();
                    }

                    valeursInstantaneesDesSixCapteurs.Add(Convert.ToInt32(nombreCourant));

                    if (valeursInstantaneesDesSixCapteurs.Count == 6)
                    {
                        valeursPremierCapteur.Add(valeursInstantaneesDesSixCapteurs[0]);
                        valeursDeuxiemeCapteur.Add(valeursInstantaneesDesSixCapteurs[1]);
                        valeursTroisiemeCapteur.Add(valeursInstantaneesDesSixCapteurs[2]);
                        valeursQuatriemeCapteur.Add(valeursInstantaneesDesSixCapteurs[3]);
                        valeursCinquiemeCapteur.Add(valeursInstantaneesDesSixCapteurs[4]);
                        valeursSixiemeCapteur.Add(valeursInstantaneesDesSixCapteurs[5]);
                    }

                    dateHeurePrecedente = dateHeureCourante;                    
                }
            }

            var fluxFichierCible = new StreamWriter(fichierCible);
            fluxFichierCible.AutoFlush = true;

            EcrireDansFichier(fluxFichierCible, "1er capteur", valeursPremierCapteur);
            EcrireDansFichier(fluxFichierCible, "2ème capteur", valeursDeuxiemeCapteur);
            EcrireDansFichier(fluxFichierCible, "3ème capteur", valeursTroisiemeCapteur);
            EcrireDansFichier(fluxFichierCible, "4ème capteur", valeursQuatriemeCapteur);
            EcrireDansFichier(fluxFichierCible, "5ème capteur", valeursCinquiemeCapteur);
            EcrireDansFichier(fluxFichierCible, "6ème capteur", valeursSixiemeCapteur);

            fluxFichierCible.Dispose();
            fluxFichierCible.Close();
        }

        private static void EcrireDansFichier(StreamWriter fluxFichierCible, string libelleCapteur, List<int> valeurs)
        {
            fluxFichierCible.WriteLine(libelleCapteur + " :");
            fluxFichierCible.WriteLine("\tMin : " + valeurs.Min().ToString());
            fluxFichierCible.WriteLine("\tMax : " + valeurs.Max().ToString());
            fluxFichierCible.WriteLine("\tMoy : " + Convert.ToInt32(valeurs.Average()).ToString());
        }
    }
}
