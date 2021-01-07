using System;
using System.Reflection;

namespace Smart.Platform.Reflection.Activation
{
    /// <summary>
    /// Provides methods for object activation.
    /// </summary>
    public class Activator
    {
        # region Constructors

        private Activator()
        {
        }

        # endregion

        # region Public Static Members

        /// <summary>
        /// Loads the type.
        /// </summary>
        /// <param name="assemblyNameClassFullName">Full name of the assembly name and class name.</param>
        /// <returns>The type.</returns>
        public static Type LoadType(string assemblyNameClassFullName)
        {
            if (assemblyNameClassFullName == null) throw new ArgumentNullException("assemblyNameClassFullName");

            // Split the string in to the separate parts
            string[] a = assemblyNameClassFullName.Split(new char[] { ',' });
            if (a.Length != 2) throw new ApplicationException(Resources.Activator.InvalidAssemblyNameClassName);

            return LoadType(a[0].Trim(), a[1].Trim());
        }

        /// <summary>
        /// Loads the type.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="className">The class name.</param>
        /// <returns>The type.</returns>
        public static Type LoadType(string assemblyName, string classFullName)
        {
            if (assemblyName == null)       throw new ArgumentNullException("assemblyName");
            if (assemblyName.Length == 0)   throw new ArgumentException("assemblyName");
            if (classFullName == null) throw new ArgumentNullException("classFullName");
            if (classFullName.Length == 0) throw new ArgumentException("classFullName");

            Assembly assembly = Assembly.Load(assemblyName);
            return assembly.GetType(classFullName);
        }

        /// <summary>
        /// Loads the instance.
        /// </summary>
        /// <param name="assemblyNameClassFullName">Full name of the assembly name and class name.</param>
        /// <returns>The object instance.</returns>
        public static object LoadInstance(string assemblyNameClassFullName)
        {
            if (assemblyNameClassFullName == null) throw new ArgumentNullException("assemblyNameClassFullName");
            if (assemblyNameClassFullName.Length == 0) throw new ArgumentException("assemblyNameClassFullName");

            return System.Activator.CreateInstance(LoadType(assemblyNameClassFullName));
        }

        /// <summary>
        /// Loads the instance.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="className">The class name.</param>
        /// <returns>The object instance.</returns>
        public static object LoadInstance(string assemblyName, string classFullName)
        {
            if (assemblyName == null)       throw new ArgumentNullException("assemblyName");
            if (assemblyName.Length == 0)   throw new ArgumentException("assemblyName");
            if (classFullName == null) throw new ArgumentNullException("classFullName");
            if (classFullName.Length == 0) throw new ArgumentException("classFullName");

            return System.Activator.CreateInstance(LoadType(assemblyName, classFullName));
        }

        /// <summary>
        /// Loads the instance using the specified parameters.
        /// </summary>
        /// <param name="assemblyNameClassFullName">Full name of the assembly name and class name.</param>
        /// <param name="args">The args.</param>
        /// <returns>The object instance.</returns>
        public static object LoadInstance(string assemblyNameClassFullName, object[] args)
        {
            if (assemblyNameClassFullName == null) throw new ArgumentNullException("assemblyNameClassFullName");
            if (assemblyNameClassFullName.Length == 0) throw new ArgumentException("assemblyNameClassFullName");
            if (args == null) throw new ArgumentNullException("args");

            return System.Activator.CreateInstance(LoadType(assemblyNameClassFullName), args);
        }

        /// <summary>
        /// Loads the instance using the specified parameters.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        /// <param name="classFullName">Full name of the class.</param>
        /// <param name="args">The args.</param>
        /// <returns>The object instance.</returns>
        public static object LoadInstance(string assemblyName, string classFullName, object[] args)
        {
            if (assemblyName == null)       throw new ArgumentNullException("assemblyName");
            if (assemblyName.Length == 0)   throw new ArgumentException("assemblyName");
            if (classFullName == null) throw new ArgumentNullException("classFullName");
            if (classFullName.Length == 0) throw new ArgumentException("classFullName");
            if (args == null) throw new ArgumentNullException("args");

            return System.Activator.CreateInstance(LoadType(assemblyName, classFullName), args);
        }

        # endregion
    }
}
