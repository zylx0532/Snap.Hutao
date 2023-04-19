﻿// Copyright (c) DGP Studio. All rights reserved.
// Licensed under the MIT license.

using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Snap.Hutao.Service;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Windows.Globalization;
using WinRT;

[assembly:InternalsVisibleTo("Snap.Hutao.Test")]

namespace Snap.Hutao;

/// <summary>
/// Program class
/// </summary>
[SuppressMessage("", "SH001")]
public static partial class Program
{
    [LibraryImport("Microsoft.ui.xaml.dll")]
    private static partial void XamlCheckProcessRequirements();

    [STAThread]
    private static void Main(string[] args)
    {
        _ = args;

        XamlCheckProcessRequirements();
        ComWrappersSupport.InitializeComWrappers();

        // by adding the using statement, we can dispose the injected services when we closing
        using (ServiceProvider serviceProvider = InitializeDependencyInjection())
        {
            InitializeCulture(serviceProvider);

            // In a Desktop app this runs a message pump internally,
            // and does not return until the application shuts down.
            Application.Start(InitializeApp);
            Control.ScopedPage.DisposePreviousScope();
        }
    }

    private static void InitializeApp(ApplicationInitializationCallbackParams param)
    {
        ThreadHelper.Initialize();
        _ = Ioc.Default.GetRequiredService<App>();
    }

    private static void InitializeCulture(IServiceProvider serviceProvider)
    {
        AppOptions appOptions = serviceProvider.GetRequiredService<AppOptions>();
        appOptions.PreviousCulture = CultureInfo.CurrentCulture;

        CultureInfo cultureInfo = appOptions.CurrentCulture;

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;
        ApplicationLanguages.PrimaryLanguageOverride = cultureInfo.Name;
    }

    private static ServiceProvider InitializeDependencyInjection()
    {
        ServiceProvider services = new ServiceCollection()

            // Microsoft extension
            .AddLogging(builder => builder.AddDebug())
            .AddMemoryCache()

            // Hutao extensions
            .AddJsonOptions()
            .AddDatabase()
            .AddInjections()
            .AddHttpClients()

            // Discrete services
            .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)

            .BuildServiceProvider(true);

        Ioc.Default.ConfigureServices(services);
        return services;
    }
}