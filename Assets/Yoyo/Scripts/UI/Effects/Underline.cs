using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yoyo.UI
{
	public class Underline : Line
	{
		private float m_fHeight;
		protected Underline()
		{
		}

		protected override int priority
		{
			get
			{
				return base.priority;
			}

			set
			{
				base.priority = value + 10000;
			}
		}

        public override void SetParameter(string value)
        {
            
        }

		protected override void OnParameterRebuild()
		{
			this.m_fHeight = ParseFloat(this.parameter,2);
		}
		protected override void ProcessCharactersAtLine(VertexHelper vh, int lineIndex, int startCharIdx, int endCharIdx, IList<UILineInfo> lines, IList<UICharInfo> chars)
		{
			var line = lines[lineIndex];
			var bottom = (line.topY - line.height) / richText.pixelsPerUnit + 1;

			var yMax = bottom;
			var yMin = bottom - this.m_fHeight;

			//var yMin = (line.topY - line.height) / richText.pixelsPerUnit + 1;
			//var yMax = yMin + 2;

			Draw(vh, startCharIdx, endCharIdx, yMin, yMax, chars);
		}
	}
}
