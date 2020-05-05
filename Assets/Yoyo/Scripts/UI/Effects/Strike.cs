using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Yoyo.UI
{
	public class Strike : Line
	{
		private float m_fHeight;
		protected Strike()
		{
		}

		protected override void OnParameterRebuild()
		{
			this.m_fHeight = TextEffect.ParseFloat(this.parameter, 2);
		}

		protected override void ProcessCharactersAtLine(VertexHelper vh, int lineIndex, int startCharIdx, int endCharIdx, IList<UILineInfo> lines, IList<UICharInfo> chars)
		{
			var line = lines[lineIndex];
			//var yMin = (line.topY - line.height * 0.6f) / richText.pixelsPerUnit;
			//var yMax = yMin + 4;
			var halfHeight = this.m_fHeight * 0.5f;
			var yMin = (line.topY - line.height * 0.5f) / richText.pixelsPerUnit - halfHeight;
			var yMax = yMin + this.m_fHeight;

			Draw(vh, startCharIdx, endCharIdx, yMin, yMax, chars);
		}
		public override void SetParameter(string value)
		{
			
		}
	}
}
