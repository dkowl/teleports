﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LibNoise.Unity.Operator
{
    /// <summary>
    /// Provides a noise module that caches the last output value generated by a source
    /// module. [OPERATOR]
    /// </summary>
    public class Cache : ModuleBase
    {
        #region Fields

        private double m_value = 0.0;
        private bool m_cached = false;
        private double m_x = 0.0;
        private double m_y = 0.0;
        private double m_z = 0.0;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of Cache.
        /// </summary>
        public Cache()
            : base(1)
        {
        }

        /// <summary>
        /// Initializes a new instance of Cache.
        /// </summary>
        /// <param name="inpu">The input module.</param>
        public Cache(ModuleBase input)
            : base(1)
        {
            this.m_modules[0] = input;
        }

        #endregion

        #region ModuleBase Members

        /// <summary>
        /// Gets or sets a source module by index.
        /// </summary>
        /// <param name="index">The index of the source module to aquire.</param>
        /// <returns>The requested source module.</returns>
        public override ModuleBase this[int index]
        {
            get { return base[index]; }
            set
            {
                base[index] = value;
                this.m_cached = false;
            }
        }

        /// <summary>
        /// Returns the output value for the given input coordinates.
        /// </summary>
        /// <param name="x">The input coordinate on the x-axis.</param>
        /// <param name="y">The input coordinate on the y-axis.</param>
        /// <param name="z">The input coordinate on the z-axis.</param>
        /// <returns>The resulting output value.</returns>
        public override double GetValue(double x, double y, double z)
        {
            System.Diagnostics.Debug.Assert(this.m_modules[0] != null);
            if (!(this.m_cached && this.m_x == x && this.m_y == y && this.m_z == z))
            {
                this.m_value = this.m_modules[0].GetValue(x, y, z);
                this.m_x = x;
                this.m_y = y;
                this.m_z = z;
            }
            this.m_cached = true;
            return this.m_value;
        }

        #endregion
    }
}