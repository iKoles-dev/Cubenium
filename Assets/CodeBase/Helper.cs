using UnityEngine;

namespace CodeBase
{
    public static class Helper
    {
        public static bool IsIntersecting(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4) => 
            IsPointsOnDifferentSides(p1, p2, p3, p4) && IsPointsOnDifferentSides(p3, p4, p1, p2);
        private static bool IsPointsOnDifferentSides(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
        {
            bool isOnDifferentSides = false;
	
            Vector3 lineDir = p2 - p1;

            Vector3 lineNormal = new Vector3(-lineDir.z, lineDir.y, lineDir.x);
	
            float dot1 = Vector3.Dot(lineNormal, p3 - p1);
            float dot2 = Vector3.Dot(lineNormal, p4 - p1);

            if (dot1 * dot2 < 0f)
            {
                isOnDifferentSides = true;
            }

            return isOnDifferentSides;
        }
    }
}