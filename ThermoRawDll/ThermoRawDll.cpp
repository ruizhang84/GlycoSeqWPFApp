// ThermoRawDll.cpp : 定义 DLL 应用程序的导出函数。
//

#include "stdafx.h"
#include "InitThermoRaw.h"
#include "ThermoRawDll.h"



//https://stackoverflow.com/questions/8971486/com-interop-how-to-use-icustommarshaler-to-call-3rd-party-component
//https://msdn.microsoft.com/en-us/library/ms235636.aspx
//https://github.com/smith-chem-wisc/mzLib/blob/master

using namespace System::Runtime::InteropServices;

ThermoDLL::ThermoDLLClass::ThermoDLLClass() {

}

ThermoDLL::PrecursorInfo ThermoDLL::ThermoDLLClass::GetPrecursorInfo(int scanNum, String^ path)
{
	MSFileReaderLib::IXRawfile5Ptr raw;
	raw.CreateInstance(__uuidof(MSFileReader_XRawfile));

	ThermoDLL::PrecursorInfo info;
	pin_ptr<const wchar_t> pathChar = static_cast<wchar_t*>(System::Runtime::InteropServices::Marshal::StringToHGlobalUni(path).ToPointer());

	raw->Open(pathChar);
	raw->SetCurrentController(0, 1);


	_variant_t vPrecursorInfos;
	long nPrecursorInfos = -1;
	raw->GetPrecursorInfoFromScanNum(scanNum, &vPrecursorInfos, &nPrecursorInfos);

	BYTE* pData;
	SafeArrayAccessData(vPrecursorInfos.parray, (void**)&pData);


	if (nPrecursorInfos > 0)
	{
		MS_PrecursorInfo precursorInfo;
		memcpy(&precursorInfo,
			pData,
			sizeof(MS_PrecursorInfo));
		info = safe_cast<PrecursorInfo>(Marshal::PtrToStructure(IntPtr(&precursorInfo), PrecursorInfo::typeid));
	}

	SafeArrayUnaccessData(vPrecursorInfos.parray);
	raw->Close();
	return info;
}



