//---------------------------------------------------------------------------

#include <vcl.h>
#pragma hdrstop

#include "Unit1.h"
//---------------------------------------------------------------------------
#pragma package(smart_init)
#pragma resource "*.dfm"
TForm1 *Form1;
#define WM_SEND_STRUCT WM_USER +1
#define WM_SEND_STRING WM_USER +2
#define WM_SEND_INT32 WM_USER +3
//---------------------------------------------------------------------------
__fastcall TForm1::TForm1(TComponent* Owner)
    : TForm(Owner)
{
}
//---------------------------------------------------------------------------

void __fastcall TForm1::Button1Click(TObject *Sender)
{
COPYDATASTRUCT MyCDS;
MyCDS.dwData=
MtCDS.cbData=sizeof(int);

      SendMessage( hwDispatch,
                   WM_COPYDATA,
                   (WPARAM)(HWND) hWnd,
                   (LPARAM) (LPVOID) &MyCDS );

/*
    int lParam = 0;
    int wParam = 99;
PSECURITY_DESCRIPTOR pSD;

pSD = (PSECURITY_DESCRIPTOR)GlobalAlloc(
         GMEM_FIXED,
         sizeof(PSECURITY_DESCRIPTOR));

if( pSD == NULL )
{
   // Handle error condition.
}


GlobalFree(pSD);


    int* data = (int*)GlobalAlloc(GMEM_FIXED, sizeof(int));

    if (data == NULL){
        // fail
        return;
    }

    *data = 99;


    HWND hWnd = FindWindow("WindowsForms10.Window.8.app.0.378734a", "Form1");
    if (hWnd != NULL){
        ::SendMessage( hWnd, WM_SEND_INT32, (WPARAM)data, 0);
    }
    GlobalFree(data);
*/
}
//---------------------------------------------------------------------------
