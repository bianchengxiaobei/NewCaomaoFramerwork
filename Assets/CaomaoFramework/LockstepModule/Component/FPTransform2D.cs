using System;
using System.Collections.Generic;

namespace CaomaoFramework
{
    public class FPTransform2D
    {
        private FPVector2 m_position;
        private FP m_rotation;
        private FPVector3 m_scale;


        public FPVector2 Position
        {
            get
            {
                return this.m_position;
            }
            set
            {
                this.m_position = value;
            }
        }

        public FPVector3 Scale
        {
            get
            {
                return this.m_scale;
            }
            set
            {
                this.m_scale = value;
            }
        }

        public FP Rotation
        {
            get
            {
                return m_rotation;
            }
            set
            {
                this.m_rotation = value;
            }
        }
    }
}
