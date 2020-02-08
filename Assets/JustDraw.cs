using System.Collections.Generic;
using UnityEngine;

public class JustDraw : MonoBehaviour
{
   //Declare serializables
   [SerializeField]
   private udcObjectPoolTransforms ObjectPool = new udcObjectPoolTransforms(100000, .1f);

   //Declare privates
   private List<Matrix4x4> lstStandardMatrices    = new List<Matrix4x4>( );
   private int             intListOfMatricesCount = 0;     //Because count is slow

   //Declare object pooling
   [System.Serializable]
   private class udcObjectPool
   {
      //Declare
      public Mesh     TheMesh;
      public Material TheMaterial;
      public int      NumberOfObjects;
   }

   

   [System.Serializable]     //Needed or else it won't show the class at all
   private class udcObjectPoolTransforms : udcObjectPool
   {
      //Declare
      [System.NonSerialized]
      public Vector3[] Positions;
      [System.NonSerialized]
      public Quaternion[] Rotations;
      [System.NonSerialized]
      public bool[] CanDraws;
      //Constructor
      public udcObjectPoolTransforms(int intNumberOfObjects, float fltXIncrease)
          : base( )
      {
         
         //Declare
         float fltX = 0f;
         //Set
         NumberOfObjects = intNumberOfObjects;
         //Expand
         Positions = new Vector3[NumberOfObjects];
         Rotations = new Quaternion[NumberOfObjects];
         CanDraws  = new bool[NumberOfObjects];
         //Loop
         for(int intLoop = 0; intLoop < NumberOfObjects; intLoop++) {
            //Set can draw
            CanDraws[intLoop] = true;
            //Set position
            Positions[intLoop] = new Vector3(fltX, 0f, 0f);
            //Increase
            fltX += fltXIncrease;
            //Set rotation
            Rotations[intLoop] = Quaternion.identity;
         }
      }
   }

   //Declare constants
   private const int intMAX_GRAPHIC_INSTANCES = 1024 - 1;     //Because of count - 1

   private void Update( )
   {
      //print(ObjectPool.Positions.Length);
      //Loop
      for(int intLoop = 0; intLoop < ObjectPool.NumberOfObjects; intLoop++) {
         //Check if can draw
         if(ObjectPool.CanDraws[intLoop]) {
            //Declare
            Matrix4x4 m4x4Object = Matrix4x4.TRS(ObjectPool.Positions[intLoop], ObjectPool.Rotations[intLoop], Vector3.one);
            //Check if maxed graphics
            if(intListOfMatricesCount >= intMAX_GRAPHIC_INSTANCES) {
               //Graphics draw mesh instanced
               GraphicsDrawMeshInstanced(ObjectPool);
            }
            //Add to list of matrices
            AddToListOfMatrices(m4x4Object);
         }
         //Randomly turn on or off draw
         //ObjectPool.CanDraws[intLoop] = Random.Range(0, 2) == 0;
      }
      //Check if there is something to draw
      if(intListOfMatricesCount > 0) {
         //Graphics draw mesh instanced
         GraphicsDrawMeshInstanced(ObjectPool);
      }
   }

   private void AddToListOfMatrices(Matrix4x4 m4x4Matrix)
   {
      //Add to list of matrices
      lstStandardMatrices.Add(m4x4Matrix);
      //Increase
      intListOfMatricesCount += 1;
   }

   private void GraphicsDrawMeshInstanced(udcObjectPoolTransforms udcObjectPoolTransformsToBe)
   {
      //Draw
      Graphics.DrawMeshInstanced(udcObjectPoolTransformsToBe.TheMesh, 0, udcObjectPoolTransformsToBe.TheMaterial, lstStandardMatrices);     //Max 1024
      //Reset
      intListOfMatricesCount = 0;
      //Empty lists
      lstStandardMatrices.Clear( );
   }
}
