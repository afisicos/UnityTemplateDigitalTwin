using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

static class PolygonDrawer
{
    public static Mesh CreateMesh(List<Vector3> _tVertex, float _fHeight = 0)
    {
        List<Vector3> tVertex = _tVertex.Select(x => new Vector3(x.x, _fHeight, x.z)).ToList();
        _tVertex.RemoveAt(_tVertex.Count - 1);
        List<Vector2> tPoints = new List<Vector2>();
        for (int i = 0; i < _tVertex.Count; i++)
        {
            tPoints.Add(ToVector2xz(_tVertex[i]));
        }
        List<int> tIndices = Triangulator.Triangulate(tPoints);
        Mesh oMesh = new Mesh();

        int iVertexCount = tVertex.Count;
        for (int index = 0; index < iVertexCount; index++)
        {
            Vector3 vVector = tVertex[index];
            tVertex.Add(new Vector3(vVector.x, _fHeight, vVector.z));
        }

        for (int i = 0; i < iVertexCount - 1; i++)
        {
            tIndices.Add(i);
            tIndices.Add(i + iVertexCount);
            tIndices.Add(i + iVertexCount + 1);
            tIndices.Add(i);
            tIndices.Add(i + iVertexCount + 1);
            tIndices.Add(i + 1);
        }

        tIndices.Add(iVertexCount - 1);
        tIndices.Add(iVertexCount);
        tIndices.Add(0);

        tIndices.Add(iVertexCount - 1);
        tIndices.Add(iVertexCount + iVertexCount - 1);
        tIndices.Add(iVertexCount);

        List<int> reverseIndices = new List<int>();
        for (int i = tIndices.Count -1; i >= 0; i--)
        {
            reverseIndices.Add(tIndices[i]);
        }
        tIndices.AddRange(reverseIndices);


        oMesh.vertices = tVertex.ToArray();
        oMesh.triangles = tIndices.ToArray();

        return oMesh;
    }


    static Vector2 ToVector2xz(Vector3 _vVector)
    {
        return new Vector2(_vVector.x, _vVector.z);
    }
    
}
