#include <stdio.h>
#include <math.h>

#define MAX_R 10000
#define MAX_C 10000
#define MAX_W 10000
#define MAX_P 10000
#define MAX_D 1000

/********global shit********/
typedef	struct drone_s
{
	int	prods[MAX_P];
}	drone_t;
drone_t drones[MAX_D];

typedef struct cust_s
{
	int r;
	int c;
	int prods[MAX_P];
	float ok_wh[MAX_W];
}	cust_t;
cust_t custs[MAX_W];

typedef struct whouse_s
{
	int r;
	int c;
	int prods[MAX_P];
}			whouse_t;
whouse_t whouses[MAX_W];

int		comands;
int		prodw[MAX_P];
int		R, C;
int		whouses_count;
int		custs_count;
int		turns_count;
int		drones_count;
int		prods_count;
int		w_max;

FILE * in;
FILE * out;
/****************************/

/************protoIO********/
void	read_env(void);
void	read_prods(void);
void	read_whouse(int);
void	print_whouse(int);
void	read_whouses(void);
void	print_whouses(void);
void	read_cust(int);
void	print_cust(int);
void	read_custs(void);
void	print_custs(void);
void	read_all(void);
void	print_all(void);
void	load(FILE*, int, int, int, int);
void	unload(FILE*, int, int, int, int);
void	deliver(FILE*, int, int, int, int);
void	wait(FILE*, int, int);
/*************************/

/******prots utils*******/
int		dist(int, int, int, int);
int		contains(int, int);
int		needs(int, int);
int		is_satisfied(int);
void	construct_needs(int);
void	satisfy_needs(int);
void	satisfy_all(void);
void	do_pack(int, int);
void	deliver_drone(int, int);
/******************/

int main(int argc, char **argv)
{	
	if (argc <= 1)
	{
		printf("UPSSS!!1!\n");
		return (1);
	}
	out = stdout;
	in = fopen(argv[1], "r");
	read_all();
//	print_all();
	satisfy_all();
	printf("\n%d\n", comands);
	fclose(in);
	return (0);
}

void	construct_needs(int cust_id)
{
	for (int i = 0; i < whouses_count; ++i)
	{
		for (int j = 0; j < prods_count; ++j)
		{
			if (contains(i, j) && needs(cust_id, j))
			{
				custs[cust_id].ok_wh[i] += (float)contains(i, j) / dist(whouses[i].r, whouses[i].c, custs[cust_id].r, custs[cust_id].c);
			}
		}
	}
}

int		is_not_satisfied(int cust_id)
{
	for (int i = 0; i < prods_count; ++i)
	{
		if (custs[cust_id].prods[i])
			return (1);
	}
	return (0);
}

void	satisfy_all(void)
{
	for (int i = 0; i < custs_count; ++i)
	{
		satisfy_needs(i);
	}
}

void	satisfy_needs(int cust_id)
{
	//construct_needs(cust_id);
	while (is_not_satisfied(cust_id))
	{
		for (int i = 0; i < drones_count; ++i)
		{
			do_pack(i, cust_id);
			deliver_drone(i, cust_id);
		}
	}
}

void	do_pack(int drone_id, int cust_id)
{
	int total_load = 0;
	int need;
	int ok;
	
	ok = 1;
	while ((total_load <= w_max) && ok)
	{
		ok = 0;
		for (int i = 0; i < prods_count; ++i)
		{
			
			if (needs(cust_id, i))
			{
				need = (w_max - total_load) / prodw[i];
				if (need == 0)
					continue;
				ok = 1;
				for (int j = 0; j < whouses_count; ++j)
				{
					if ((contains(j, i)) && (total_load <= w_max))
					{
						if (contains(j, i) < needs(cust_id, i))
						{
							if (need >= contains(j, i))
							{
								load(out, drone_id, j, i, contains(j, i));
								total_load += prodw[i] * contains(j, i);
								custs[cust_id].prods[i] -= contains(j, i);
								whouses[j].prods[i] = 0;
							}
							else
							{
								load(out, drone_id, j, i, need);
								total_load += prodw[i] * need;
								custs[cust_id].prods[i] -= need;
								whouses[j].prods[i] -= need;
							}
						}
						else
						{
							if (need >= needs(cust_id, i))
							{
								load(out, drone_id, j, i, needs(cust_id, i));
								total_load += prodw[i] * needs(cust_id, i);
								custs[cust_id].prods[i] = 0;
								whouses[j].prods[i] -= needs(cust_id, i);
							}
							else
							{
								load(out, drone_id, j, i, need);
								total_load += prodw[i] * need;
								custs[cust_id].prods[i] -= need;
								whouses[j].prods[i] -= need;
							}
							
						}
					}
				}
			}
		}
	}
}

void	deliver_drone(int drone_id, int cust_id)
{
	for (int i = 0; i < prods_count; ++i)
	{
		if (drones[drone_id].prods[i])
			deliver(out, drone_id, cust_id, i, drones[drone_id].prods[i]);
	}
}

/********UTILS************/
int		dist(int r1, int c1, int r2, int c2)
{
	return (ceil(sqrt((r1 - r2) * (r1 - r2) + (c1 - c2) * (c1 - c2))));
}

int		contains(int whouse_id, int prod_id)
{
	return (whouses[whouse_id].prods[prod_id]);
}

int		needs(int cust_id, int prod_id)
{
	return (custs[cust_id].prods[prod_id]);
}
/*************************/


/**************************/
/************IO************/

void	load(FILE * out, int drone_id, int whouse_id, int prod_id, int prod_c)
{
	comands++;
	drones[drone_id].prods[prod_id] += prod_c;
	printf("%d L %d %d %d\n", drone_id, whouse_id, prod_id, prod_c);
}

void	unload(FILE * out, int drone_id, int whouse_id, int prod_id, int prod_c)
{
	comands++;
	drones[drone_id].prods[prod_id] -= prod_c;
	printf("%d U %d %d %d\n", drone_id, whouse_id, prod_id, prod_c);
}

void	deliver(FILE * out, int drone_id, int cust_id, int prod_id, int prod_c)
{
	comands++;
	drones[drone_id].prods[prod_id] -= prod_c;
	printf("%d D %d %d %d\n", drone_id, cust_id, prod_id, prod_c);
}

void	wait(FILE *out, int drone_id, int trns)
{
	comands++;
	fprintf(out, "%d W %d\n", drone_id, trns);
}

void	read_env(void)
{
	fscanf(in, "%d %d %d %d %d\n", &R, &C, &drones_count, &turns_count, &w_max);
//	printf("env:\n%d %d %d %d %d\n", R, C, drones_count, turns_count, w_max);
}


void	read_prods(void)
{
	char aux;

	fscanf(in, "%d\n", &prods_count);
	for (int i = 0; i < prods_count; ++i)
	{
		fscanf(in, "%d", prodw + i);
//		printf("%d ", prodw[i]);
	}
	fscanf(in, "%c", &aux);
//	printf("\n");
}

void	read_whouse(int index)
{
	char aux;

	fscanf(in, "%d %d\n", &((whouses[index]).r), &((whouses[index]).c));
	for (int i = 0; i < prods_count; ++i)
	{
		fscanf(in, "%d", &((whouses[index]).prods[i]));
	}
	fscanf(in, "%c", &aux);
}

void	print_whouse(int index)
{
	printf("whouse: %d \n[%d, %d]\n", index, whouses[index].r, whouses[index].c);
	for (int i = 0; i < prods_count; ++i)
	{
		printf("%d ", whouses[index].prods[i]);
	}
	printf("\n\n");
}

void	read_whouses(void)
{
	fscanf(in, "%d\n", &whouses_count);
	for (int i = 0; i < whouses_count; ++i)
	{
		read_whouse(i);
	}
}

void	print_whouses(void)
{
	for (int i = 0; i < whouses_count; ++i)
	{
		print_whouse(i);
	}
}

void	read_cust(int index)
{
	int		auxi, local_count;
	char	auxc;
	

	fscanf(in, "%d %d\n", &(custs[index].r), &(custs[index].c));
	fscanf(in, "%d\n", &local_count);
	for (int i = 0; i < local_count; ++i)
	{
		fscanf(in, "%d", &auxi);
		custs[index].prods[auxi]++;
	}
	fscanf(in, "%c", &auxc);
}

void	print_cust(int index)
{
	printf("customer: %d\n[%d, %d]\n", index, custs[index].r, custs[index].c);
	for (int i = 0; i < prods_count; ++i)
	{
		printf("%d ", custs[index].prods[i]);
	}
	printf("\n");
	for (int i = 0; i < whouses_count; ++i)
	{
		printf("%f ", custs[index].ok_wh[i]);
	}
	printf("\n\n");
}

void	read_custs(void)
{
	fscanf(in, "%d\n", &custs_count);
	for (int i = 0; i < custs_count; ++i)
	{
		read_cust(i);
	}
}

void	print_custs(void)
{
	for (int i = 0; i < custs_count; ++i)
	{
		print_cust(i);
	}
}

void	read_all(void)
{
	read_env();
	read_prods();
	read_whouses();
	read_custs();
}

void	print_all(void)
{
	print_whouses();
	print_custs();
}
/***************************/
/***************************/
