using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Teleports {

    public static class ColorUtils {

        public static readonly Vector3[] Yuv2RgbMatrix =
        {
            new Vector3(1, 0, 1.28033f ),
            new Vector3(1, -0.21482f, -0.38059f ),
            new Vector3(1, 2.12798f, 0 )
        };

        public static readonly Vector3[] SpheresInCube16 = ToUnitSpace(new Vector3[]
            {
                new Vector3(-0.0850628f,      -0.1180330f,       0.4575529f),
                new Vector3(0.6224065f,       0.0884086f,      0.6224065f),
                new Vector3(0.0884086f,       0.6224065f,       0.6224065f),
                new Vector3(-0.6224065f,      -0.6224065f,       0.6224064f),
                new Vector3(-0.6224065f,       0.4122055f,       0.4779590f),
                new Vector3(0.4779590f,      -0.6224065f,       0.4122055f),
                new Vector3(0.6224065f,       0.6224065f,       0.0884086f),
                new Vector3(-0.6224064f,      -0.1043786f,      -0.0729024f),
                new Vector3(-0.0729024f,      -0.6224064f,      -0.1043787f),
                new Vector3(-0.1180330f,       0.4575529f,      -0.0850628f),
                new Vector3(0.4575529f,      -0.0850628f,      -0.1180330f),
                new Vector3(-0.6224065f,       0.6224064f,      -0.6224065f),
                new Vector3(-0.6224065f,      -0.6224065f,      -0.6224065f),
                new Vector3(0.6224064f,      -0.6224065f,      -0.6224065f),
                new Vector3(0.4122055f,       0.4779590f,      -0.6224065f),
                new Vector3(-0.1043786f,      -0.0729024f,      -0.6224064f),
            },
            new Vector2(-1, 1)
        );

        public static Vector3 MatrixByVector(Vector3[] m, Vector3 v)
        {
            Vector3 result = new Vector3();
            result.x = Vector3.Dot(m[0], v);
            result.y = Vector3.Dot(m[1], v);
            result.z = Vector3.Dot(m[2], v);
            return result;
        }

        public static Vector3[] ToUnitSpace(Vector3[] m, Vector2 otherSpace)
        {
            float add = -otherSpace.x;
            float scale = 1 / (otherSpace.y - otherSpace.x);
            Vector3[] result = new Vector3[m.Length];
            for(int i = 0; i<result.Length; i++)
            {
                Vector3 newV = m[i] + new Vector3(add, add, add);
                newV.Scale(new Vector3(scale, scale, scale));
                result[i] = newV;
            }
            return result;
        }

        public static Color ColorFromVector3(Vector3 v)
        {
            return new Color(v.x, v.y, v.z);
        }

        public static Color YUV2RGB(Vector3 yuv)
        {
            return new Color(
                1.164f*(yuv.x - 1f/16f) + 1.596f*(yuv.z - 1f/2f),
                1.164f*(yuv.x - 1f/16f) - 0.813f*(yuv.z - 1f/2f) - 0.391f*(yuv.y - 1f/2f),
                1.164f*(yuv.x - 1f/16f) + 2.018f*(yuv.y - 1f/2f)
                );
        }

        public static Color[] DistinctColors16()
        {
            Color[] result = new Color[16];
            for(int i = 0; i<16; i++)
            {
                //result[i] = ColorFromVector3(SpheresInCube16[i]);
                result[i] = YUV2RGB(SpheresInCube16[i]);
            }
            return result;
        }
    }
}
