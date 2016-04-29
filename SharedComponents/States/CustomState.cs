using System;
using System.Collections.Generic;

namespace AntMe.SharedComponents.States {
    /// <summary>
    /// Holds custom <c>Plugin</c>-Information
    /// </summary>
    [Serializable]
    public sealed class CustomState {
        private readonly List<string> keys = new List<string>();
        private readonly List<object> values = new List<object>();

        /// <summary>
        /// Gets a value indicating whether this instance has value.
        /// </summary>
        /// <value><c>true</c> if this instance has value; otherwise, <c>false</c>.</value>
        public bool HasValue {
            get { return keys != null && keys.Count > 0; }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add(string key, object value) {
            if (key == null) {
                throw new ArgumentNullException("key");
            }
            if (value == null) {
                throw new ArgumentNullException("value");
            }

            if (keys.Contains(key)) {
                int index = keys.IndexOf(key);
                values[index] = value;
            }
            else {
                keys.Add(key);
                values.Add(value);
            }
            keys.TrimExcess();
            values.TrimExcess();
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public bool Remove(string key) {
            if (key == null) {
                return false;
            }

            int index = keys.IndexOf(key);
            if (index == -1) {
                return false;
            }
            keys.Remove(key);
            values.RemoveAt(index);
            keys.TrimExcess();
            values.TrimExcess();
            return true;
        }

        /// <summary>
        /// Removes the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public bool Remove(object value) {
            if (value == null) {
                return false;
            }
            int index = values.IndexOf(value);
            if (index == -1) {
                return false;
            }
            values.Remove(value);
            keys.RemoveAt(index);
            values.TrimExcess();
            keys.TrimExcess();
            return true;
        }

        /// <summary>
        /// Gets or sets the <see cref="System.Object"/> with the specified key.
        /// </summary>
        /// <value></value>
        public object this[string key] {
            get {
                if (!keys.Contains(key)) {
                    return null;
                }
                int index = keys.IndexOf(key);
                return values[index];
            }
            set { Add(key, value); }
        }

        /// <summary>
        /// Gets a list of available keys.
        /// </summary>
        public string[] Keys {
            get { return keys.ToArray(); }
        }
    }
}