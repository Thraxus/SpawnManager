using System;
using System.Collections.Generic;
using Sandbox.Definitions;
using Sandbox.Game.Entities;
using VRage.Game;
using VRage.Game.Entity;
using VRage.Utils;
using VRageMath;

namespace SpawnManager.Tools
{
    public class SpawnPoint
    {

        /// <summary>
        /// Takes a point and radius and returns all entities found within the sphere
        /// </summary>
        /// <param name="detectionCenter">Point of scan origin</param>
        /// <param name="range">How far from the origin you want to scan for entities</param>
        /// <returns>An enumerable list of entities</returns>
        public IEnumerable<MyEntity> DetectEntitiesInSphere(Vector3D detectionCenter, double range)
        {
            BoundingSphereD pruneSphere = new BoundingSphereD(detectionCenter, range);
            List<MyEntity> pruneList = new List<MyEntity>();
            MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref pruneSphere, pruneList, MyEntityQueryType.Dynamic);
            return pruneList;
        }

        public Vector3D DetectEntitiesInBox(MyObjectBuilder_CubeGrid grid, Vector3D spawnOrigin)
        {
            BoundingBoxD box = CalculateBoundingBox(grid);
            box.Translate(spawnOrigin);
            List<MyEntity> pruneList;
            do
            {
                pruneList = new List<MyEntity>();
                MyGamePruningStructure.GetAllEntitiesInBox(ref box, pruneList, MyEntityQueryType.Both);
                if (pruneList.Count <= 0) continue;
                foreach (MyEntity entity in pruneList)
                {
                    Core.GeneralLog.WriteToLog("DetectEntitiesInBox", $"{entity}");
                }
                //box = box.Translate(new Vector3D(10, 0, 0));
                //box = box.Translate(new Vector3D(.5, .5, .5));
                box = box.Translate(Vector3D.Up + .25);
            } while (pruneList.Count > 0);
            //MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref pruneSphere, pruneList, MyEntityQueryType.Dynamic);
            return box.Center;
        }

        private BoundingBox CalculateBoundingBox(MyObjectBuilder_CubeGrid grid)
        {
            float cubeSize = MyDefinitionManager.Static.GetCubeSize(grid.GridSizeEnum);
            BoundingBox boundingBox = new BoundingBox(Vector3.MaxValue, Vector3.MinValue);
            try
            {
                foreach (MyObjectBuilder_CubeBlock cubeBlock in grid.CubeBlocks)
                {
                    MyCubeBlockDefinition blockDefinition;
                    if (!MyDefinitionManager.Static.TryGetCubeBlockDefinition(cubeBlock.GetId(), out blockDefinition))
                        continue;
                    MyBlockOrientation blockOrientation = (MyBlockOrientation)cubeBlock.BlockOrientation;
                    Vector3 vector3 = Vector3.Abs(Vector3.TransformNormal(new Vector3(blockDefinition.Size) * cubeSize, blockOrientation));
                    Vector3 point1 = new Vector3((Vector3I)cubeBlock.Min) * cubeSize - new Vector3(cubeSize / 2f);
                    Vector3 point2 = point1 + vector3;
                    boundingBox.Include(point1);
                    boundingBox.Include(point2);
                }
            }
            catch (Exception e)
            {
	            Core.GeneralLog.WriteToLog("CalculateBoundingBox", $"Exception:\t{e}");
                return new BoundingBox();
            }
            return boundingBox;
        }

        public bool SpaceCollisionDetection(Vector3D detectionCenter)
        {
            BoundingSphereD locationSphere = new BoundingSphereD(detectionCenter, 75);
            List<MyEntity> pruneList = new List<MyEntity>();
            MyGamePruningStructure.GetAllTopMostEntitiesInSphere(ref locationSphere, pruneList, MyEntityQueryType.Both);
            return pruneList.Count > 0;
        }

        public static void DrawBox(MyOrientedBoundingBoxD obb, Color color)
        {
            BoundingBoxD box = new BoundingBoxD(-obb.HalfExtent, obb.HalfExtent);
            MatrixD wm = MatrixD.CreateFromTransformScale(obb.Orientation, obb.Center, Vector3D.One);
            MySimpleObjectDraw.DrawTransparentBox(ref wm, ref box, ref color, MySimpleObjectRasterizer.Solid, 1);
        }

        public static void DrawObb(MatrixD matrix, MyOrientedBoundingBoxD obb, Color color)
        {
            BoundingBoxD box = new BoundingBoxD(-obb.HalfExtent, obb.HalfExtent);
            MySimpleObjectDraw.DrawTransparentBox(ref matrix, ref box, ref color, MySimpleObjectRasterizer.Solid, 1);
        }

        public static void DrawBox3(MatrixD matrix, BoundingBoxD box, Color color, MySimpleObjectRasterizer raster = MySimpleObjectRasterizer.Wireframe, float thickness = 0.01f)
        {
            MySimpleObjectDraw.DrawTransparentBox(ref matrix, ref box, ref color, raster, 1, thickness, MyStringId.GetOrCompute("Square"), MyStringId.GetOrCompute("Square"));
        }

        public static void DrawObb(MyOrientedBoundingBoxD obb, Color color, MySimpleObjectRasterizer raster = MySimpleObjectRasterizer.Wireframe, float thickness = 0.01f)
        {
            BoundingBoxD box = new BoundingBoxD(-obb.HalfExtent, obb.HalfExtent);
            MatrixD wm = MatrixD.CreateFromQuaternion(obb.Orientation);
            wm.Translation = obb.Center;
            MySimpleObjectDraw.DrawTransparentBox(ref wm, ref box, ref color, raster, 1, thickness, MyStringId.GetOrCompute("Square"), MyStringId.GetOrCompute("Square"));
        }

        private void DrawDebugBox()
        {
            MyQuadD myQuadD = new MyQuadD
            {
                Point0 = new Vector3D(),
                Point1 = new Vector3D(),
                Point2 = new Vector3D(),
                Point3 = new Vector3D()
            };
            //MySimpleObjectDraw.DrawTransparentBox();

            //MyTransparentGeometry
            //MyTransparentGeometry
            //MySimpleObjectDraw.DrawAttachedTransparentBox();
            //MyEntities.
            //public static void DrawAttachedTransparentBox(ref MatrixD worldMatrix, ref BoundingBoxD localbox, ref Color color, uint renderObjectID, ref MatrixD worldToLocal, MySimpleObjectRasterizer rasterization, int wireDivideRatio, float lineWidth = 1f, MyStringId? faceMaterial = null, MyStringId? lineMaterial = null, bool onlyFrontFaces = false)
        }

    }
}
