﻿using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;

namespace SQLDefuzzer
{
    /// <summary>
    /// This is the class that implements the package exposed by this assembly.
    /// </summary>
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [Guid(SQLDefuzzerPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideOptionPage(typeof(GeneralOptions), "SQLDefuzzer", "General", 0, 0, true)]
    [ProvideOptionPage(typeof(SQLFLUFFOptions), "SQLDefuzzer", "SQLFLUFF", 0, 0, true)]
    public sealed class SQLDefuzzerPackage : Package
    {
        /// <summary>
        /// SQLDefuzzerPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "91e1c149-6e51-41ca-8a5d-0e138a51fd7b";

        #region Package Members

        /// <summary>
        /// Initialization of the package; this method is called right after the package is sited, so this is the place
        /// where you can put all the initialization code that rely on services provided by VisualStudio.
        /// </summary>
        /// <param name="cancellationToken">A cancellation token to monitor for initialization cancellation, which can occur when VS is shutting down.</param>
        /// <param name="progress">A provider for progress updates.</param>
        /// <returns>A task representing the async work of package initialization, or an already completed task if there is none. Do not return null from this method.</returns>
        protected override void Initialize()
        {
            // When initialized asynchronously, the current thread may be a background thread at this point.
            // Do any initialization that requires the UI thread after switching to the UI thread.
            //await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);
            DeFuzz.Initialize(this);
            base.Initialize();
        }

        #endregion
    }
}
