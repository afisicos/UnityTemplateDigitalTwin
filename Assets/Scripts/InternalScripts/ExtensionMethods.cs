using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.EventSystems;

public static class ExtensionMethods
{
    public static T GetRandom<T>(this List<T> _tList)
    {
        return _tList[UnityEngine.Random.Range(0, _tList.Count)];
    }
    public static T GetRandomAndRemove<T>(this List<T> _tList)
    {
        T randomOne = _tList[UnityEngine.Random.Range(0, _tList.Count)];
        _tList.Remove(randomOne);
        return randomOne;
    }

    public static T GetPonderatedRandom<T>(this List<T> _tList, List<float> _tWeights)
    {
        float fTotal = 0;
        for (int i = 0; i < _tWeights.Count; i++)
        {
            fTotal += _tWeights[i];
        }
        float fRandomValue = UnityEngine.Random.Range(0, fTotal);
        float fCurrent = 0;
        for (int i = 0; i < _tWeights.Count; i++)
        {
            fCurrent += _tWeights[i];
            if (fCurrent >= fRandomValue)
            {
                return _tList[i];
            }
        }
        return default(T);
    }
    
    public static bool InsidePolygon(Vector3 _vPoint, List<Vector3> _tPolygon)
    {
        bool bResult = false;
        int j = _tPolygon.Count - 1;
        for (int i = 0; i < _tPolygon.Count; i++)
        {
            if (_tPolygon[i].z < _vPoint.z && _tPolygon[j].z >= _vPoint.z || _tPolygon[j].z < _vPoint.z && _tPolygon[i].z >= _vPoint.z)
            {
                if (_tPolygon[i].x + (_vPoint.z - _tPolygon[i].z) / (_tPolygon[j].z - _tPolygon[i].z) * (_tPolygon[j].x - _tPolygon[i].x) < _vPoint.x)
                {
                    bResult = !bResult;
                }
            }
            j = i;
        }
        return bResult;
    }
    public static bool InsidePolygon(Vector3 _vPoint, List<Vector3> _tPolygon, List<float> _tDivisionProducts)
    {
        bool bResult = false;
        int j = _tPolygon.Count - 1;
        for (int i = 0; i < _tPolygon.Count; i++)
        {
            if (_tPolygon[i].z < _vPoint.z && _tPolygon[j].z >= _vPoint.z || _tPolygon[j].z < _vPoint.z && _tPolygon[i].z >= _vPoint.z)
            {
                if (_tPolygon[i].x + (_vPoint.z - _tPolygon[i].z) * _tDivisionProducts[i] < _vPoint.x)
                {
                    bResult = !bResult;
                }
            }
            j = i;
        }
        return bResult;
    }

    public static bool PointInsideRect(Vector2 _vPoint, RectTransform _tRectT)
    {

        Rect rect = new Rect();
        rect.Set(_tRectT.position.x - _tRectT.rect.width / 2, _tRectT.position.y - _tRectT.rect.height / 2, _tRectT.rect.width, _tRectT.rect.height);

        if (rect.Contains(_vPoint))
        {
            return true;
        }
        else return false;
    }

    public static Vector3 PointInSegmentClosestToOther(Vector3 _vPoint, Vector3 _vSegmentStart, Vector3 _vSegmentEnd)
    {
        Vector2 vResult = PointInSegmentClosestToOther(new Vector2(_vPoint.x, _vPoint.z), new Vector2(_vSegmentStart.x, _vSegmentStart.z), new Vector2(_vSegmentEnd.x, _vSegmentEnd.z));
        return new Vector3(vResult.x, 0, vResult.y);
    }

    public static Vector2 PointInSegmentClosestToOther(Vector2 _vPoint, Vector2 _vSegmentStart, Vector2 _vSegmentEnd)
    {
        Vector2 vVectorV = _vSegmentEnd - _vSegmentStart;
        Vector2 vVectorU = _vSegmentStart - _vPoint;

        float fValueT = -(Vector2.Dot(vVectorV, vVectorU) / Vector2.Dot(vVectorV, vVectorV));

        return Vector2.Lerp(_vSegmentStart, _vSegmentEnd, fValueT);
    }

    public static float GetDistPointToSegment(float ax, float ay, float bx,
                             float by, float x, float y)
    {
        if ((ax - bx) * (x - bx) + (ay - by) * (y - by) <= 0)
            return Mathf.Sqrt((x - bx) * (x - bx) + (y - by) * (y - by));

        if ((bx - ax) * (x - ax) + (by - ay) * (y - ay) <= 0)
            return Mathf.Sqrt((x - ax) * (x - ax) + (y - ay) * (y - ay));

        return Mathf.Abs((by - ay) * x - (bx - ax) * y + bx * ay - by * ax) /
            Mathf.Sqrt((ay - by) * (ay - by) + (ax - bx) * (ax - bx));
    }

    public static string GetTimeText(int _iSeconds)
    {
        int iHours = _iSeconds / 3600;

        int iNonHours = _iSeconds % 3600;
        int iMinutes = iNonHours / 60;

        if (iHours > 0)
        {
            return iHours + " h " + iMinutes + " min";

        }
        else if (iMinutes > 0)
        {
            int iSeconds = iNonHours % 60;
            return iMinutes + " min " + iSeconds + " s";
        }
        else
        {
            return _iSeconds + " s";
        }
    }

    public static int ClosestPointFromList(List<Vector3> _tPoints, Vector3 _vReference)
    {
        float fClosestDistance = 999;
        int iClosest = 0;
        for (int i = 0; i < _tPoints.Count; i++)
        {
            if (Vector3.SqrMagnitude(_tPoints[i] - _vReference) < fClosestDistance)
            {
                fClosestDistance = Vector3.SqrMagnitude(_tPoints[i] - _vReference);
                iClosest = i;
            }
        }
        return iClosest;
    }

    public static Vector3 ClosestVertexFromList(List<Vector3> points, Vector3 reference)
    {
        float fClosestDistance = 999;
        Vector3 vClosest = new Vector3();
        for (int i = 0; i < points.Count; i++)
        {
            if (Vector3.SqrMagnitude(points[i] - reference) < fClosestDistance)
            {
                fClosestDistance = Vector3.SqrMagnitude(points[i] - reference);
                vClosest = points[i];
            }
        }
        return vClosest;
    }

    public static int[] ClosestPairOfVerticesFromList(List<Vector3> points, Vector3 reference)
    {
        List<Vector3> vertices = new List<Vector3>();
        vertices.Add(points[0]);
        vertices.Add(points[1]);
        vertices.Add(points[2]);
        vertices.Add(points[3]);
        float fClosestDistance1 = 999;
        float fClosestDistance2 = 999;
        int iClosest1 = 0;
        int iClosest2 = 0;

        for (int i = 0; i < vertices.Count; i++)
        {
            if (Vector3.SqrMagnitude(vertices[i] - reference) < fClosestDistance1)
            {
                fClosestDistance1 = Vector3.SqrMagnitude(vertices[i] - reference);
                iClosest1 = i;
            }
        }
        vertices.Remove(vertices[iClosest1]);

        for (int j = 0; j < vertices.Count; j++)
        {
            if (Vector3.SqrMagnitude(vertices[j] - reference) < fClosestDistance2)
            {
                fClosestDistance2 = Vector3.SqrMagnitude(vertices[j] - reference);
                iClosest2 = j;
            }
        }
        iClosest2 = iClosest1 <= iClosest2 ? iClosest2 + 1 : iClosest2;
        return new int[2] { iClosest1, iClosest2 };
    }

    //Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement()
    {
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }


    //Returns 'true' if we touched or hovering on Unity UI element.
    private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
                return true;
        }
        return false;
    }
    //Gets all event system raycast results of current mouse or touch position.
    public static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public static Mesh GeneratePlanarMesh(MeshFilter _oFilter, List<Vector3> _tVertex, float _fMeshHeight)
    {
        Mesh oMesh = 
            PolygonDrawer.CreateMesh(_tVertex, _fMeshHeight);
        _oFilter.mesh = oMesh;
        Vector2[] uvs = new Vector2[oMesh.vertices.Length];
        List<Vector3> tNormals = new List<Vector3>();
        for (int i = 0; i < uvs.Length; i++)
        {
            uvs[i] = new Vector2(oMesh.vertices[i].x, oMesh.vertices[i].z);
            tNormals.Add(Vector3.up);
        }
        oMesh.uv = uvs;
        oMesh.normals = tNormals.ToArray();
        return oMesh;
    }
    public static Mesh GenerateWallMesh(MeshFilter _oFilter, List<Vector3> _tVertex, float _fMeshHeight)
    {
        List<Vector3> tVertex = new List<Vector3>();
        List<Vector3> tNormals = new List<Vector3>();
        List<int> tTriangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();


        for (int j = 0; j < _tVertex.Count; j++)
        {
            Vector3 vVertex1 = _tVertex[j];
            Vector3 vVertex2 = _tVertex[0];
            if (j < _tVertex.Count - 1)
            {
                vVertex2 = _tVertex[j + 1];
            }

            Vector3 vVertex3 = vVertex1 + Vector3.up * _fMeshHeight;
            Vector3 vVertex4 = vVertex2 + Vector3.up * _fMeshHeight;

            tVertex.Add(vVertex1);
            tVertex.Add(vVertex2);
            tVertex.Add(vVertex3);
            tVertex.Add(vVertex4);

            Vector3 vWayCentre = GetCentre(_tVertex);

            tNormals.Add(vVertex1 - vWayCentre);
            tNormals.Add(vVertex2 - vWayCentre);
            tNormals.Add(vVertex3 - vWayCentre);
            tNormals.Add(vVertex4 - vWayCentre);

            int i1, i2, i3, i4;

            i4 = tVertex.Count - 1;
            i3 = tVertex.Count - 2;
            i2 = tVertex.Count - 3;
            i1 = tVertex.Count - 4;

            tTriangles.Add(i1);
            tTriangles.Add(i3);
            tTriangles.Add(i4);

            tTriangles.Add(i1);
            tTriangles.Add(i4);
            tTriangles.Add(i2);

            tTriangles.Add(i2);
            tTriangles.Add(i3);
            tTriangles.Add(i1);

            tTriangles.Add(i2);
            tTriangles.Add(i4);
            tTriangles.Add(i3);

            float repeatX = Vector3.Distance(vVertex1, vVertex2);

            uvs.Add(new Vector2(0, 0));
            uvs.Add(new Vector2(0, repeatX * 0.1f));
            uvs.Add(new Vector2(_fMeshHeight * 0.1f, 0));
            uvs.Add(new Vector2(_fMeshHeight * 0.1f, repeatX * 0.1f));

        }

        //Walls mesh
        Mesh oWallsMesh = new Mesh();
        _oFilter.mesh = oWallsMesh;
        oWallsMesh.vertices = tVertex.ToArray();
        oWallsMesh.triangles = tTriangles.ToArray();

        oWallsMesh.uv = uvs.ToArray();

        oWallsMesh.normals = tNormals.ToArray();

        return oWallsMesh;
    }

    public static Vector3 GetCentre(List<Vector3> _tVertex)
    {
        Vector3 vTotal = Vector3.zero;

        foreach (var xVer in _tVertex)
        {
            vTotal += xVer;

        }
        return vTotal / _tVertex.Count;
    }

    public static float CalculateAreaOfMesh(Mesh _oMesh)
    {
        var tTriangles = _oMesh.triangles;
        var tVertices = _oMesh.vertices;

        double dSum = 0.0;

        for (int i = 0; i < tTriangles.Length; i += 3)
        {
            Vector3 vCorner = tVertices[tTriangles[i]];
            Vector3 vA = tVertices[tTriangles[i + 1]] - vCorner;
            Vector3 vB = tVertices[tTriangles[i + 2]] - vCorner;

            dSum += Vector3.Cross(vA, vB).magnitude;
        }

        return (float)(dSum / 2.0);
    }

    public static void StretchQuad(Transform _oQuad, Vector3 _vStart, Vector3 _vEnd, float _fWidth = 0.3f)
    {
        _oQuad.position = Vector3.Lerp(_vStart, _vEnd, 0.5f);
        _oQuad.LookAt(_vEnd);
        _oQuad.Rotate(90, 0, 0);
        _oQuad.localScale = new Vector3(_fWidth, Vector3.Distance(_vStart, _vEnd), 1);
    }

    public static Color HSLtoRGB(float _fHue, float _fSaturation, float _fLightness)
    {
        float c, hh, x, rr = 0, gg = 0, bb = 0, m;
        if (_fHue < 0)
        {
            _fHue += 360;
        }
        c = (1 - Mathf.Abs(2 * _fLightness - 1)) * _fSaturation;
        hh = _fHue / 60;
        x = c * (1 - Mathf.Abs(hh % 2 - 1));
        if (hh < 1)
        {
            rr = c;
            gg = x;
            bb = 0;
        }
        else if (hh < 2)
        {
            rr = x;
            gg = c;
            bb = 0;
        }
        else if (hh < 3)
        {
            rr = 0;
            gg = c;
            bb = x;
        }
        else if (hh < 4)
        {
            rr = 0;
            gg = x;
            bb = c;
        }
        else if (hh < 5)
        {
            rr = x;
            gg = 0;
            bb = c;
        }
        else if (hh < 6)
        {
            rr = c;
            gg = 0;
            bb = x;
        }
        m = _fLightness - c / 2;
        return new Color(rr + m, gg + m, bb + m);

    }
    public static float[] HSLFromRGB(Color color)
    {
        return HSLFromRGB(color.r, color.g, color.b);
    }



    public static float[] HSLFromRGB(float R, float G, float B)
    {
        float Min = Mathf.Min(Mathf.Min(R, G), B);
        float Max = Mathf.Max(Mathf.Max(R, G), B);
        float Chroma = Max - Min;

        float hue = 0;
        float saturation = 0;
        float luminosity = (float)((Max + Min) / 2.0f);
        if (luminosity < 1f)
        {
            saturation = (float)(Chroma / (1 - Mathf.Abs(2 * luminosity - 1)));
        }
        else
        {
            saturation = 0;
        }

        if (Chroma != 0)
        {
            if (R == Max)
            {
                hue = 60 * (((G - B) / Chroma) % 6);
            }
            else if (G == Max)
            {
                hue = 60 * (2f + (B - R) / Chroma);
            }
            else if (B == Max)
            {
                hue = 60 * (4f + (R - G) / Chroma);
            }
        }
        else
        {
            hue = 0;
        }

        if (hue < 0)
        {
            hue += 360;
        }

        return new float[] { hue, saturation, luminosity };
    }
    public static float GetHue(Color _oColor)
    {
        return GetHue(_oColor.r, _oColor.g, _oColor.b);
    }
    public static float GetHue(float R, float G, float B)
    {

        float Min = Mathf.Min(Mathf.Min(R, G), B);
        float Max = Mathf.Max(Mathf.Max(R, G), B);
        float Chroma = Max - Min;

        float hue = 0;

        if (Chroma != 0)
        {
            if (R == Max)
            {
                hue = 60 * (((G - B) / Chroma) % 6);
            }
            else if (G == Max)
            {
                hue = 60 * (2f + (B - R) / Chroma);
            }
            else if (B == Max)
            {
                hue = 60 * (4f + (R - G) / Chroma);
            }
        }
        else
        {
            hue = 0;
        }

        if (hue < 0)
        {
            hue += 360;
        }

        return hue;
    }


    public static List<System.Numerics.Vector3> ConvertVector3Format(List<Vector3> vectors)
    {
        List<System.Numerics.Vector3> targetList = new List<System.Numerics.Vector3>();
        foreach (Vector3 vector in vectors)
        {
            targetList.Add(new System.Numerics.Vector3(vector.x, vector.y, vector.z));
        }
        return targetList;
    }

    public static string GenerarClaveHexadecimal(int longitud)
    {
        // Usar un generador de números aleatorios
        System.Random random = new System.Random();

        // Crear un array de bytes para almacenar la clave
        byte[] bytes = new byte[longitud / 2];

        // Llenar el array de bytes con valores aleatorios
        random.NextBytes(bytes);

        // Convertir los bytes a su representación hexadecimal
        string claveHexadecimal = BitConverter.ToString(bytes).Replace("-", "").Substring(0, longitud);

        return claveHexadecimal;
    }
}