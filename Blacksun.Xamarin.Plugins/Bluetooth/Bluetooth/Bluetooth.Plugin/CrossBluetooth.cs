﻿using System;
using Plugin.Bluetooth.Abstractions;

namespace Plugin.Bluetooth
{
    public class CrossBluetooth
    {
        static Lazy<IBluetooth> Implementation = new Lazy<IBluetooth>(() => CreateBluetooth(), System.Threading.LazyThreadSafetyMode.PublicationOnly);

        /// <summary>
        /// Current settings to use
        /// </summary>
        public static IBluetooth Current
        {
            get
            {
                var ret = Implementation.Value;
                if (ret == null)
                {
                    throw NotImplementedInReferenceAssembly();
                }
                return ret;
            }
        }

        static IBluetooth CreateBluetooth()
        {
#if PORTABLE
            return null;
#else
            return new BluetoothImplementation();
#endif
        }

        internal static Exception NotImplementedInReferenceAssembly()
        {
            return new NotImplementedException("This functionality is not implemented in the portable version of this assembly.  You should reference the NuGet package from your main application project in order to reference the platform-specific implementation.");
        }


        /// <summary>
        /// Dispose of everything 
        /// </summary>
        public static void Dispose()
        {
            if (Implementation != null && Implementation.IsValueCreated)
            {
                Implementation.Value.Dispose();

                Implementation = new Lazy<IBluetooth>(() => CreateBluetooth(), System.Threading.LazyThreadSafetyMode.PublicationOnly);
            }
        }
    }
}
