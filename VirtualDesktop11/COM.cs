﻿// Author: Markus Scholtes, 2021
// Version 1.9, 2021-10-08
// Version for Windows 10 21H2 and Windows 11
// Compile with:
// C:\Windows\Microsoft.NET\Framework\v4.0.30319\csc.exe VirtualDesktop11.cs
// Based on http://stackoverflow.com/a/32417530, Windows 10 SDK, github project Grabacr07/VirtualDesktop and own research

/////////////////////////////////////////////////
// Dylan Cheng (https://github.com/newlooper)
// added Notifications about desktop
// added conditional compile for C#/WinRT breaking change on .NET 5.0+

using System;
using System.Runtime.InteropServices;

namespace VirtualDesktop
{
    internal static class Guids
    {
        public static readonly Guid CLSID_ImmersiveShell                    = new( "C2F03A33-21F5-47FA-B4BB-156362A2F239" );
        public static readonly Guid CLSID_VirtualDesktopManagerInternal     = new( "C5E0CDCA-7B6E-41B2-9FC4-D93975CC467B" );
        public static readonly Guid CLSID_VirtualDesktopManager             = new( "AA509086-5CA9-4C25-8F95-589D3C07B48A" );
        public static readonly Guid CLSID_VirtualDesktopPinnedApps          = new( "B5A399E7-1C87-46B8-88E9-FC5747B171BD" );
        public static readonly Guid CLSID_VirtualDesktopNotificationService = new( "A501FDEC-4A09-464C-AE4E-1B9C21B84918" );
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct Size
    {
        public int X;
        public int Y;
    }

    [StructLayout( LayoutKind.Sequential )]
    public struct Rect
    {
        public int Left;
        public int Top;
        public int Right;
        public int Bottom;
    }

    public enum APPLICATION_VIEW_CLOAK_TYPE : int
    {
        AVCT_NONE            = 0,
        AVCT_DEFAULT         = 1,
        AVCT_VIRTUAL_DESKTOP = 2
    }

    public enum APPLICATION_VIEW_COMPATIBILITY_POLICY : int
    {
        AVCP_NONE                = 0,
        AVCP_SMALL_SCREEN        = 1,
        AVCP_TABLET_SMALL_SCREEN = 2,
        AVCP_VERY_SMALL_SCREEN   = 3,
        AVCP_HIGH_SCALE_FACTOR   = 4
    }

    [ComImport]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "372E1D3B-38D3-42E4-A15B-8AB2B178F513" )]
    public interface IApplicationView
    {
        void GetIIdsSlot();
        void GetRuntimeClassNameSlot();
        void GetTrustLevelSlot();
        int  SetFocus();
        int  SwitchTo();
        int  TryInvokeBack( IntPtr /* IAsyncCallback* */ callback );
        int  GetThumbnailWindow( out IntPtr hWnd );
        int  GetMonitor( out         IntPtr /* IImmersiveMonitor */ immersiveMonitor );
        int  GetVisibility( out      int visibility );
        int  SetCloak( APPLICATION_VIEW_CLOAK_TYPE cloakType, int unknown );
        int  GetPosition( ref Guid guid /* GUID for IApplicationViewPosition */, out IntPtr /* IApplicationViewPosition** */ position );
        int  SetPosition( ref IntPtr /* IApplicationViewPosition* */ position );
        int  InsertAfterWindow( IntPtr hWnd );
        int  GetExtendedFramePosition( out                              Rect rect );
        int  GetAppUserModelId( [MarshalAs( UnmanagedType.LPWStr )] out string id );
        int  SetAppUserModelId( string id );
        int  IsEqualByAppUserModelId( string id, out int result );
        int  GetViewState( out uint state );
        int  SetViewState( uint state );
        int  GetNeediness( out               int neediness );
        int  GetLastActivationTimestamp( out ulong timestamp );
        int  SetLastActivationTimestamp( ulong timestamp );
        int  GetVirtualDesktopId( out Guid guid );
        int  SetVirtualDesktopId( ref Guid guid );
        int  GetShowInSwitchers( out  int flag );
        int  SetShowInSwitchers( int flag );
        int  GetScaleFactor( out             int factor );
        int  CanReceiveInput( out            bool canReceiveInput );
        int  GetCompatibilityPolicyType( out APPLICATION_VIEW_COMPATIBILITY_POLICY flags );
        int  SetCompatibilityPolicyType( APPLICATION_VIEW_COMPATIBILITY_POLICY flags );
        int  GetSizeConstraints( IntPtr /* IImmersiveMonitor* */ monitor, out Size size1, out Size size2 );
        int  GetSizeConstraintsForDpi( uint uint1, out Size size1, out Size size2 );
        int  SetSizeConstraintsForDpi( ref uint uint1, ref Size size1, ref Size size2 );
        int  OnMinSizePreferencesUpdated( IntPtr hWnd );
        int  ApplyOperation( IntPtr /* IApplicationViewOperation* */ operation );
        int  IsTray( out                  bool isTray );
        int  IsInHighZOrderBand( out      bool isInHighZOrderBand );
        int  IsSplashScreenPresented( out bool isSplashScreenPresented );
        int  Flash();
        int  GetRootSwitchableOwner( out                              IApplicationView rootSwitchableOwner );
        int  EnumerateOwnershipTree( out                              IObjectArray     ownershipTree );
        int  GetEnterpriseId( [MarshalAs( UnmanagedType.LPWStr )] out string           enterpriseId );
        int  IsMirrored( out                                          bool             isMirrored );
        int  Unknown1( out                                            int              unknown );
        int  Unknown2( out                                            int              unknown );
        int  Unknown3( out                                            int              unknown );
        int  Unknown4( out                                            int              unknown );
        int  Unknown5( out                                            int              unknown );
        int  Unknown6( int                                                             unknown );
        int  Unknown7();
        int  Unknown8( out int   unknown );
        int  Unknown9( int       unknown );
        int  Unknown10( int      unknownX, int unknownY );
        int  Unknown11( int      unknown );
        int  Unknown12( out Size size1 );
    }

    [ComImport]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "1841C6D7-4F9D-42C0-AF41-8747538F10E5" )]
    public interface IApplicationViewCollection
    {
        int  GetViews( out         IObjectArray array );
        int  GetViewsByZOrder( out IObjectArray array );
        int  GetViewsByAppUserModelId( string   id,          out IObjectArray     array );
        int  GetViewForHWnd( IntPtr             hWnd,        out IApplicationView view );
        int  GetViewForApplication( object      application, out IApplicationView view );
        int  GetViewForAppUserModelId( string   id,          out IApplicationView view );
        int  GetViewInFocus( out IntPtr         view );
        int  Unknown1( out       IntPtr         view );
        void RefreshCollection();
        int  RegisterForApplicationViewChanges( object listener, out int cookie );
        int  UnregisterForApplicationViewChanges( int  cookie );
    }

    [ComImport]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "536D3495-B208-4CC9-AE26-DE8111275BF8" )]
    public interface IVirtualDesktop
    {
        bool   IsViewVisible( IApplicationView view );
        Guid   GetId();
        IntPtr Unknown1();

#if NET5_0_OR_GREATER
        public void GetString( out IntPtr hstr );
#else
        [return: MarshalAs( UnmanagedType.HString )]
        string GetName();
#endif

        [return: MarshalAs( UnmanagedType.HString )]
        string GetWallpaperPath();
    }

    [ComImport]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "B2F925B9-5A0F-4D2E-9F4D-2B1507593C10" )]
    internal interface IVirtualDesktopManagerInternal
    {
        int             GetCount( IntPtr                      hWndOrMon );
        void            MoveViewToDesktop( IApplicationView   view, IVirtualDesktop desktop );
        bool            CanViewMoveDesktops( IApplicationView view );
        IVirtualDesktop GetCurrentDesktop( IntPtr             hWndOrMon );

        /////////////////////////////////////////////////////////////////
        // COM Interface "B2F925B9-5A0F-4D2E-9F4D-2B1507593C10"
        // for windows 11 22H2 (>22489) COM Interface require this func.
        // for windows 11 21H2 (22000.xxx) this func must be commented
#if WIN22H2
        IObjectArray GetAllCurrentDesktops();
#endif
        void         GetDesktops( IntPtr hWndOrMon, out IObjectArray desktops );

        [PreserveSig]
        int GetAdjacentDesktop( IVirtualDesktop from, int direction, out IVirtualDesktop desktop );

        void            SwitchDesktop( IntPtr                                hWndOrMon, IVirtualDesktop desktop );
        IVirtualDesktop CreateDesktop( IntPtr                                hWndOrMon );
        void            MoveDesktop( IVirtualDesktop                         desktop, IntPtr          hWndOrMon, int nIndex );
        void            RemoveDesktop( IVirtualDesktop                       desktop, IVirtualDesktop fallback );
        IVirtualDesktop FindDesktop( ref Guid                                desktopId );
        void            GetDesktopSwitchIncludeExcludeViews( IVirtualDesktop desktop, out IObjectArray unknown1, out IObjectArray unknown2 );
#if NET5_0_OR_GREATER
        void SetName( IVirtualDesktop      desktop, IntPtr newName );
        void SetWallpaper( IVirtualDesktop desktop, IntPtr path );
#else
        void SetDesktopName( IVirtualDesktop      desktop, [MarshalAs( UnmanagedType.HString )] string name );
        void SetDesktopWallpaper( IVirtualDesktop desktop, [MarshalAs( UnmanagedType.HString )] string path );
#endif
        void UpdateWallpaperPathForAllDesktops( [MarshalAs( UnmanagedType.HString )] string path );
        void CopyDesktopState( IApplicationView                                             pView0, IApplicationView pView1 );
        int  GetDesktopIsPerMonitor();
        void SetDesktopIsPerMonitor( bool state );
    }

    [ComImport]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "A5CD92FF-29BE-454C-8D04-D82879FB3F1B" )]
    internal interface IVirtualDesktopManager
    {
        bool IsWindowOnCurrentVirtualDesktop( IntPtr topLevelWindow );
        Guid GetWindowDesktopId( IntPtr              topLevelWindow );
        void MoveWindowToDesktop( IntPtr             topLevelWindow, ref Guid desktopId );
    }

    [ComImport]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "4CE81583-1E4C-4632-A621-07A53543148F" )]
    internal interface IVirtualDesktopPinnedApps
    {
        bool IsAppIdPinned( string          appId );
        void PinAppID( string               appId );
        void UnpinAppID( string             appId );
        bool IsViewPinned( IApplicationView applicationView );
        void PinView( IApplicationView      applicationView );
        void UnpinView( IApplicationView    applicationView );
    }

    [ComImport]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "92CA9DCD-5622-4BBA-A805-5E9F541BD8C9" )]
    public interface IObjectArray
    {
        void GetCount( out int count );
        void GetAt( int        index, ref Guid iid, [MarshalAs( UnmanagedType.Interface )] out object obj );
    }

    [ComImport]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    [Guid( "6D5140C1-7436-11CE-8034-00AA006009FA" )]
    internal interface IServiceProvider10
    {
        [return: MarshalAs( UnmanagedType.IUnknown )]
        object QueryService( ref Guid service, ref Guid riid );
    }

    [ComImport]
    [Guid( "cd403e52-deed-4c13-b437-b98380f2b1e8" )]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    public interface IVirtualDesktopNotification
    {
        void VirtualDesktopCreated( IObjectArray       p0, IVirtualDesktop pDesktop );
        void VirtualDesktopDestroyBegin( IObjectArray  p0, IVirtualDesktop pDesktopDestroyed, IVirtualDesktop pDesktopFallback );
        void VirtualDesktopDestroyFailed( IObjectArray p0, IVirtualDesktop pDesktopDestroyed, IVirtualDesktop pDesktopFallback );
        void VirtualDesktopDestroyed( IObjectArray     p0, IVirtualDesktop pDesktopDestroyed, IVirtualDesktop pDesktopFallback );
        void Proc7( int                                p0 );
        void VirtualDesktopMoved( IObjectArray         p0, IVirtualDesktop pDesktop, int nIndexFrom, int nIndexTo );
#if NET5_0_OR_GREATER
        void VirtualDesktopRenamed( IVirtualDesktop pDesktop, IntPtr newName );
#else
        void VirtualDesktopRenamed( IVirtualDesktop pDesktop, [MarshalAs( UnmanagedType.HString )] string chName );
#endif
        void ViewVirtualDesktopChanged( IApplicationView pView );
        void CurrentVirtualDesktopChanged( IObjectArray  p0, IVirtualDesktop pDesktopOld, IVirtualDesktop pDesktopNew );

#if NET5_0_OR_GREATER
        void VirtualDesktopWallpaperChanged( IVirtualDesktop pDesktop, IntPtr newPath );
#else
        void VirtualDesktopWallpaperChanged( IVirtualDesktop pDesktop, [MarshalAs( UnmanagedType.HString )] string chPath );
#endif
    }

    [ComImport]
    [Guid( "0cd45e71-d927-4f15-8b0a-8fef525337bf" )]
    [InterfaceType( ComInterfaceType.InterfaceIsIUnknown )]
    public interface IVirtualDesktopNotificationService
    {
        uint Register( IVirtualDesktopNotification pNotification );
        void Unregister( uint                      dwCookie );
    }
}