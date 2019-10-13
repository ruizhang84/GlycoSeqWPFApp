#pragma once
using namespace System;

namespace ThermoDLL
{
	public value struct PrecursorInfo
	{
		double dIsolationMass;
		double dMonoIsoMass;
		long nChargeState;
		long nScanNumber;
	};


	public ref class ThermoDLLClass
	{
	public:
		ThermoDLLClass();
		PrecursorInfo GetPrecursorInfo(int scanNum, String^ path);
	};


}