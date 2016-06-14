#include "BSQ.h"
#include <stdlib.h>
#include <stdio.h>

t_map	get_map(void)
{
	t_map new;
	
	new.n = 9;
	new.m = 27;
	new.oriz = (int**)malloc(sizeof(int*) * new.n);	

	new.oriz[0] = (int*)malloc(sizeof(int));
	new.oriz[0][0] = INF;
	
	new.oriz[1] = (int*)malloc(sizeof(int) * 2);
	new.oriz[1][0] = 4;
	new.oriz[1][1] = INF;
	
	new.oriz[2] = (int*)malloc(sizeof(int) * 2);
	new.oriz[2][0] = 12;
	new.oriz[2][1] = INF;

	new.oriz[3] = (int*)malloc(sizeof(int));
	new.oriz[3][0] = INF;

	new.oriz[4] = (int*)malloc(sizeof(int) * 2);
	new.oriz[4][0] = 4;
	new.oriz[4][1] = INF;

	new.oriz[5] = (int*)malloc(sizeof(int) * 2);
	new.oriz[5][0] = 15;
	new.oriz[5][1] = INF;

	new.oriz[6] = (int*)malloc(sizeof(int));
	new.oriz[6][0] = INF;

	new.oriz[7] = (int*)malloc(sizeof(int) * 3);
	new.oriz[7][0] = 6;
	new.oriz[7][1] = 22;
	new.oriz[7][2] = INF;
	
	new.oriz[8] = (int*)malloc(sizeof(int) * 3);
	new.oriz[8][0] = 2;
	new.oriz[8][1] = 10;
	new.oriz[8][2] = INF;
	
	return (new);
}
