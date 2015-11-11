/*
    The MIT License (MIT)
    Copyright © 2015 Englishtown <opensource@englishtown.com>

    Permission is hereby granted, free of charge, to any person obtaining a copy
    of this software and associated documentation files (the "Software"), to deal
    in the Software without restriction, including without limitation the rights
    to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
    copies of the Software, and to permit persons to whom the Software is
    furnished to do so, subject to the following conditions:

    The above copyright notice and this permission notice shall be included in
    all copies or substantial portions of the Software.

    THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
    IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
    FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
    AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
    LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
    OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
    THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;

namespace EF.Diagnostics.Profiling.Timings
{
    /// <summary>
    /// Represents a collection of tags of a timing.
    /// </summary>
    [Serializable]
    public class TagCollection : HashSet<string>
    {
        private const string SeparatorToken = ",";

        #region Constructors
        
        /// <summary>
        /// Initializes a <see cref="TagCollection"/> from serialization info.
        /// </summary>
        /// <param name="info">The <see cref="SerializationInfo"/>.</param>
        /// <param name="context">The <see cref="StreamingContext"/>.</param>
        protected TagCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// Initializes a <see cref="TagCollection"/> from a list tags.
        /// </summary>
        /// <param name="tags">Initialize from a list of tags.</param>
        public TagCollection(IEnumerable<string> tags = null)
            : base(StringComparer.OrdinalIgnoreCase)
        {
            if (tags != null)
            {
                foreach (var tag in tags)
                {
                    if (tag != null)
                    {
                        var formattedTag = FormatTag(tag);
                        if (formattedTag != null)
                        {
                            Add(formattedTag);
                        }
                    }
                }
            }
        }

        #endregion

        #region Serialization

        /// <summary>
        /// Serializes the current <see cref="TagCollection"/> to string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Join(SeparatorToken.ToString(CultureInfo.InvariantCulture), this);
        }

        /// <summary>
        /// Creates a <see cref="TagCollection"/> from a serialized string generated by TagCollection.ToString()
        /// </summary>
        /// <param name="tags">The serialized string of a list of tags.</param>
        /// <returns>Returns the deserialized <see cref="TagCollection"/>.</returns>
        public static TagCollection FromString(string tags)
        {
            if (string.IsNullOrWhiteSpace(tags))
            {
                return null;
            }

            return new TagCollection(tags.Split(SeparatorToken[0]));
        }

        #endregion

        #region Private Methods

        private string FormatTag(string tag)
        {
            if (string.IsNullOrEmpty(tag))
            {
                return null;
            }

            var result = tag.Replace("\r\n", string.Empty).Replace(SeparatorToken, string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(result))
            {
                return null;
            }

            return result;
        }

        #endregion
    }
}
