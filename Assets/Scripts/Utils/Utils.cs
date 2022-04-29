using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Utils
{
    public static readonly float PhysicsDelta = 2.5f*Physics2D.defaultContactOffset;

    public static bool FloatsAreEqual(float a, float b, float epsilon = 0.001f)
    {
        bool result = (a > b - epsilon) && (a < b + epsilon);

        return result;
    }

    public static GameObject GetOrGenerateRootGO(string goName)
    {
        GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        GameObject queriedGO = rootObjects.FirstOrDefault(x => string.Compare(x.name, goName) == 0);
        if (queriedGO == null)
        {
            queriedGO = new GameObject(goName);
        }
        return queriedGO;
    }

    public static string GetGeneralWordPlural(this string singular)
    {
        string lowered = singular.ToLower();
        string termination = "";
        string result = singular;
        int toTrim = 0;
        if (!string.IsNullOrEmpty(lowered) && lowered.Length > 1)
        {
            if (lowered[lowered.Length - 2] == 'o' && lowered[lowered.Length - 1] == 'n')
            {
                toTrim = 2;
                termination = "a";
            }
            else if (lowered[lowered.Length - 1] == 'o')
            {
                termination = "es";
            }
            else if (lowered[lowered.Length - 2].IsVowel() && lowered[lowered.Length - 1] == 's')
            {
                termination = "s";
            }
            else if (!lowered[lowered.Length - 2].IsVowel() && lowered[lowered.Length - 1] == 's')
            {
                toTrim = 1;
                termination = "ies";
            }
            else if ((lowered[lowered.Length - 2] == 'i' && lowered[lowered.Length - 1] == 's') ||
                      (lowered[lowered.Length - 2] == 's' && lowered[lowered.Length - 1] == 's') ||
                      (lowered[lowered.Length - 2] == 's' && lowered[lowered.Length - 1] == 'h') ||
                      (lowered[lowered.Length - 2] == 'c' && lowered[lowered.Length - 1] == 'h'))
            {

                toTrim = 2;
                termination = "es";
            }
            else if ((lowered[lowered.Length - 1] == 's') ||
                      (lowered[lowered.Length - 1] == 'x') ||
                      (lowered[lowered.Length - 1] == 'z'))
            {

                toTrim = 1;
                termination = "es";
            }
            else
            {
                termination = "s";
            }

            result = result.Substring(0, singular.Length - toTrim);
        }

        return result + termination;
    }

    public static bool IsVowel(this char c)
    {
        char lowered = char.ToLower(c);

        return (lowered == 'a' ||
                lowered == 'e' ||
                lowered == 'i' ||
                lowered == 'o' ||
                lowered == 'u');
    }
}
