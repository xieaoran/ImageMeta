#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include <math.h>

#define byte unsigned char

double GetDistance(int r, int g, int b)
{
	long double value = r * r + g * g + b * b;
	return sqrt(value);
}

byte Average(byte* list, int length)
{
	long long sum = 0;
	for (auto i = 0; i < length; i++)
	{
		sum += list[i];
	}
	return static_cast<byte>(sum / length);
}

int IndexOfMin(double* arr, int count)
{
	double min;
	auto index = 0;
	min = arr[0];
	for (auto i = 0; i < count; i++)
	{
		if (arr[i] < min)
		{
			min = arr[i];
			index = i;
		}
	}
	return index;
}

#define SafeDelete(ptr) if(ptr){ delete ptr; ptr=NULL;}

struct metaunit
{
	unsigned long long Rs, Gs, Bs;
	byte r, g, b;
	int capacity;
	int count;

	void init(int capacity)
	{
		this->capacity = capacity;
		count = 0;
		Rs = 0;
		Gs = 0;
		Bs = 0;
	}

	void attach(byte r, byte g, byte b)
	{
		Rs += r;
		Bs += b;
		Gs += g;
		count++;
	}

	void clear()
	{
		Rs = 0;
		Gs = 0;
		Bs = 0;
		this->count = 0;
	}
};


extern "C"
void
_declspec(dllexport)
meta_calculate(byte* decodedImageRGBBytes, int byteLength, int metaCount, byte* out_colors, int maxError)
{
	auto points = new metaunit[metaCount];
	auto distance = new double[metaCount];
	auto pixelCount = byteLength / 3;
	metaunit current;

	byte r, g, b;

	for (auto i = 0; i < metaCount; i++)
	{
		points[i].init(pixelCount);
		srand(static_cast<int>(time(nullptr)));
		points[i].r = rand() % 256;
		points[i].g = rand() % 256;
		points[i].b = rand() % 256;
	}
	auto calc = true;
	while (calc)
	{
		for (auto i = 0; i < pixelCount; i++)
		{
			for (auto j = 0; j < metaCount; j++)
			{
				distance[j] = 0;
			}
			byte r1 = decodedImageRGBBytes[i * 3];
			byte g1 = decodedImageRGBBytes[i * 3 + 1];
			byte b1 = decodedImageRGBBytes[i * 3 + 2];
			for (auto pointCount = 0; pointCount < metaCount; pointCount++)
			{
				current = points[pointCount];
				byte currentR = current.r;
				byte currentG = current.g;
				byte currentB = current.b;
				distance[pointCount] = GetDistance(r1 - currentR, g1 - currentG, b1 - currentB);
			}
			points[IndexOfMin(distance, metaCount)].attach(r1, g1, b1);
		}
		for (auto p = 0; p < metaCount; p++)
		{
			current = points[p];
			auto colorCount = current.count;
			r = (colorCount != 0) ? current.Rs / colorCount : static_cast<byte>(rand() % 256);
			g = (colorCount != 0) ? current.Gs / colorCount : static_cast<byte>(rand() % 256);
			b = (colorCount != 0) ? current.Bs / colorCount : static_cast<byte>(rand() % 256);
			if (abs(r - current.r) <= maxError && abs(g - current.g) <= maxError && abs(b - current.b) <= maxError)
			{
				calc = false;
			}

			points[p].r = r;
			points[p].g = g;
			points[p].b = b;
			points[p].clear();
		}
	}
	for (auto i = 0; i < metaCount; i++)
	{
		out_colors[i * 3] = points[i].r;
		out_colors[i * 3 + 1] = points[i].g;
		out_colors[i * 3 + 2] = points[i].b;
	}
	delete[] points;
	delete[] distance;
}
