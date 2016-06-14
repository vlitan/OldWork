#include <iostream>
#include <stdio.h>
#include <fstream>
#include <vector>
#include <utility>
#include <deque>
using namespace std;

ifstream in ("flow.in");
ofstream out ("flow.out");

struct linie
{
    int coord1;
    int coord2;
} puncte[10000];

struct path
{
    int color;
    int length;
    int sp;
    char steps[10000];
} P1;

int r, c,maxcol, Matrix[10000][10000], s5[10000];//for all points

int get_i(int k);
int get_j (int k);
void primire();
void primire2();
void Print2();
int abs(int x);
int CalcMD(int xi, int xj, int yi, int yj);

void AddToMatrix(int k, int cul)
{
    Matrix[get_i(k)][get_j(k)]=cul;
}

void PrintMatrix()
{
    for(int i=1;i<=r;i++)
    {
        for(int j=1;j<=c;j++)
        {
            out<<Matrix[i][j]<<' ';
        }
        out<<'\n';
    }


}

void AddPoint(int p, int c)
{
    if(puncte[c].coord1==0)
    {
        puncte[c].coord1=p;
    }
    else
    {
        puncte[c].coord2=p;
    }

    if(c>maxcol)
        maxcol=c;

    AddToMatrix(p,c);
}



void MakeStep(int& i,int& j,char s)
{
    switch (s)
    {
    case 'N':i--;
     break;
    case 'S':i++;
     break;
    case 'W':j--;
     break;
    case 'E':j++;
     break;
    };
}
void Solve5();
bool OutOfBounds(int k,int lim)
{
    if(k<1) return 1;
    if(k>lim) return 1;

    return 0;
}

void DiscardPath(path p,int nrs)//discards first nrs steps of p
{
    cout<<"I had to discard, but I`m smart:D";
    return;
    int ic, jc,ep;
      ic=get_i(p.sp);
      jc=get_j(p.sp);
    for(int i=0;i<nrs;i++)
    {
        MakeStep(ic,jc,p.steps[i]);
        if(Matrix[ic][jc]==p.color)
            Matrix[ic][jc]=0;
        else
            break;
    }
}

pair<int, int> CheckAndAddPath(path p)
{
    /*first = length
      second = step index
      -1 stands for "something is wrong but not this"*/
      int len, stp,ic, jc,ep;
      ic=get_i(p.sp);
      jc=get_j(p.sp);
      if(puncte[p.color].coord1==p.sp)
        ep=puncte[p.color].coord2;
      else
        ep=puncte[p.color].coord1;

      for(int i=1;i<=p.length;i++)
      {
          MakeStep(ic,jc,p.steps[i]);
          if(OutOfBounds(ic,r)||OutOfBounds(jc,c))
          {
              len= -1;
              stp= i+1;
              //out<<'a'<<p.color;
              DiscardPath(p,i);
              break;
          }
          if(Matrix[ic][jc]!=0&& (ic!=get_i(ep)||jc!=get_j(ep)))
          {
              len=-2;
              stp=i+1;
              //out<<'b'<<p.color;
              DiscardPath(p,i);
              break;
          }
          Matrix[ic][jc]=p.color;

      }

      if(ic!=get_i(ep)||jc!=get_j(ep))
      {
          len=p.length;
          stp=-1;
          //out<<'c'<<p.color<<'\n';
          //out<<ic<<jc;
          DiscardPath(p,p.length);

      }
      else
      {
          len=p.length;
          stp=1;
          //out<<'d'<<p.color;
         // return make_pair(len,stp);
      }

}

void Solve4()
{

}

void primire345()
{
    int nop,k,cul,NRT;
    double nopt;
    in>>NRT;
    while(NRT--)
    {

        for(int i=0;i<=maxcol;i++)
            s5[i]=puncte[i].coord1=puncte[i].coord2=0;
        maxcol=0;
        for(int i=0;i<=r;i++)
           for(int j=0;j<=c;j++)
            {
                Matrix[i][j]=0;
            }
        in>>r>>c>>nop;
        while(nop--)
        {
            in>>k>>cul;
            AddPoint(k,cul);
        }
        in>>nopt;
        //PrintMatrix();
        while(nopt--)
        {
            int a;

            in>>P1.color;
            in>>P1.sp;
            in>>P1.length;
            for(int i=1;i<=P1.length;i++)
            {
                char step;
                in>>step;
                P1.steps[i]=step;
            }
           // out<<'\n'<<P1.color<<'\n';
            CheckAndAddPath(P1);
            s5[P1.color]=1;
           // out<<'\n';
        }
        Solve5();


    }
}

void Solve3();

bool Lee(int sp,int ep)
{

    int Sec[r][c];
    for(int i=0;i<=r;i++)
        for(int j=0;j<=c;j++)
        Sec[i][j]=0;
    int i,j;
    int I[4]={-1,0,1,0};
    int J[4]={0,1,0,-1};
    deque< pair<int,int> > dq;
    Sec[get_i(ep)][get_j(ep)]=-1;
    if(get_i(sp)>r or get_j(sp)>c)
        out<<sp<<'w'<<get_i(sp)<<"c"<<r<<c<<'\t';
    Sec[get_i(sp)][get_j(sp)]=1;
    dq.push_front(make_pair(get_i(sp),get_j(sp)));
    for(;!dq.empty();)
    {
        int ci=dq.back().first;
        int cj=dq.back().second;
        for(int k=0;k<4;k++)
        {
            int ii=ci+I[k];
            int jj=cj+J[k];
            if(!(OutOfBounds(ii,r)||OutOfBounds(jj,c)))
            {
                if(Matrix[ii][jj]==0&&Sec[ii][jj]==0)
                {
                    dq.push_front(make_pair(ii,jj));
                    Sec[ii][jj]=Sec[ci][cj]+1;
                }
                if(Sec[ii][jj]==-1)
                {
                    return 1;
                }
            }
        }
        dq.pop_back();
    }
    return 0;
}



void PrintMatrixBW()
{
        for(int i=1;i<=r;i++)
    {
        for(int j=1;j<=c;j++)
        {
            if(Matrix[i][j])
                out<<'x';
            else
                out<<' ';
        }
        out<<'\n';
    }
}

int main()///\\\///\\\///\\\///\\\///\\\///\\\///
{
    primire345();
    out<<'\n';
    //PrintMatrixBW();
    PrintMatrix();
    return 0;
}

void Solve5()
{
    for(int i=1;i<=maxcol;i++)
    {
        if(s5[i])
        {
            out<<1<<' ';
        }
        else if(Lee(puncte[i].coord1,puncte[i].coord2))
        {
            out<<2<<' ';
        }
        else
        {
            out<<3<<' ';
        }
    }
}


void Solve3()
{
    pair<int, int> r=CheckAndAddPath(P1);
    if(r.first==-1)
    {
        out<<-1<<' '<<r.second;
    }
    else if(r.first==-2)
    {
        out<<-1<<' '<<r.second;
    }
    else if(r.second==-1)
    {
        out<<-1<<' '<<r.first;
    }
    else
    {
        out<<1<<' '<<r.first;
    }
}

void Print2()
{
    for(int i=1;i<=maxcol;++i)
    {
        out<<CalcMD(get_i(puncte[i].coord1), get_j(puncte[i].coord1),get_i(puncte[i].coord2), get_j(puncte[i].coord2))<<' ';
    }
}

int CalcMD(int xi, int xj, int yi, int yj)
{
    return abs(xi-yi)+abs(xj-yj);
}


int get_j (int k)
{
    if(k%c==0) return c;
    else return k%c;
}

int get_i(int k)
{
    if(k%c==0) return k/c;
    else return k/c+1;
}

void primire()
{
    int nop, k;
    in>>r>>c>>nop;

    while(nop--)
    {
        in>>k;

        out<<get_i(k)<<" ";
        out<<get_j(k)<<" ";
    }
}

int abs(int x)
{
    return (x<0) ? -x:x;
}

void primire2()
{
    int t, k, cul;
    in>>r>>c>>t;
    while(t--)
    {
        in>>k>>cul;

        AddPoint(k,cul);
    }
}
