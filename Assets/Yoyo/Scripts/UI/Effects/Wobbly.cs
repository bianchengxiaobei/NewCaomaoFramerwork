using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;

namespace Yoyo.UI
{
	public class Wobbly : TextEffect
	{
		[SerializeField]
		private float m_Speed;
		[SerializeField]
		private float m_Density;
		[SerializeField]
		private float m_Magnitude;

		protected Wobbly()
		{
		}

		protected override void OnParameterRebuild()
		{
			var a = parameter.Split(',');
            m_Speed = ParseFloat(a.Length > 0 ? a[0] : string.Empty, 3);
            m_Density = ParseFloat(a.Length > 1 ? a[1] : string.Empty, 1);
            m_Magnitude = ParseFloat(a.Length > 2 ? a[2] : string.Empty, 2);
        }

        public override void SetParameter(string value)
        {
            
        }


        protected override void ProcessCharactersAtLine(VertexHelper vh, int lineIndex, int startCharIdx, int endCharIdx, IList<UILineInfo> lines, IList<UICharInfo> chars)
        {

            UIVertex vertex = UIVertex.simpleVert;
            //2019起作用，2018不行
            var tempStartIndex = startCharIdx;
            while (startCharIdx <= endCharIdx)
            {
                if (char.IsWhiteSpace(this.richText.text,startCharIdx) == false)
                {
                    //如果不是空白的
                    for (int i = 0; i < 4; i++)
                    {
                        var k = tempStartIndex * 4 + i;
                        vh.PopulateUIVertex(ref vertex, k);
                        //try
                        //{
                        //    vh.PopulateUIVertex(ref vertex, k);
                        //}
                        //catch (Exception e)
                        //{
                        //    Debug.LogException(e);
                        //}
                        vertex.position = vertex.position + new Vector3(0, m_Magnitude * Mathf.Sin((Time.timeSinceLevelLoad * m_Speed) + (tempStartIndex * m_Density)), 0);
                        vh.SetUIVertex(vertex, k);
                    }
                    tempStartIndex++;
                }
                ++startCharIdx;
            }

        }

        void Update()
        {
			if (richText != null) {
				richText.SetVerticesDirty();
			}
		}
	}
}
