#include <iostream>
#include <stdlib.h>
#include <fstream>
#include <cstring>
#include <stdio.h>
#include <string>
#include <math.h>
using namespace std;

struct Bar
{
    int poz;
    int mov;
}CPU,ME;

struct Ball
{
    float x;
    float y;
    float Vy;
    float Vx;
}BALL;

struct point
{
    float x;
    float y;
};

void ReadUpdate()
{
    string aux;
    cin>>aux; //player
    cerr << aux;
    cin>>ME.poz>>ME.mov;
    cin>>aux;//CPU
    cerr << aux;
    cin>>CPU.poz>>CPU.mov;
    cin>>aux;//BALL
    cerr<<aux;
    cin>>BALL.x>>BALL.y>>BALL.Vx>>BALL.Vy;
}

inline float abs(float x)
{
    return (x>0)? x:-x;
}

inline float GetY(point X, point Y)
{
    return (X.x*(Y.y-X.y)-X.y*(Y.x-X.x))/(X.x-Y.x);
}

inline float GetFinalY(point X, point Y,int n)
{
   float y=GetY(X,Y);
   if((int)trunc(y/n)%2==0)
   {
       return abs( (int)y%n);
   }
   else
   {
       return abs(n-(int)abs(y)%n);
   }
}

int GetMove(float FinalY, int Actual)// atcual= poz+mov
{
    return FinalY-Actual;
}

bool GoesAway(point x, point y)
{
    return 0>y.x-x.x;
}

int main()
{

    point x,y;
    string aux;
    int k=0;
    int ycorr=75;
    int xcorr=25;
/*
    x.x=6;
    x.y=2;
    y.x=1;
    y.y=5;
    cout<<GoesAway(x,y);*/
    int R=1;
    while(1)
    {
        x.x=BALL.x+xcorr;
        x.y=BALL.y+xcorr;
        ReadUpdate();
        if(!(cin>>aux)) break;
        y.x=BALL.x+xcorr;
        y.y=BALL.y+xcorr;
        int Y=(int)GetFinalY(x,y,600);
        cout<<"move "<<GetMove(Y,ME.poz+ycorr+ME.mov)+rand()%R-R/2<<endl;
        fprintf(stderr,"%d",10);


    }
    return 0;
}



