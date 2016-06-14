#include <stdio.h>
#include <BSQ.h>

void	print_point(t_point p)
{
	printf("x:%d y:%d\n", p.x, p.y);
}
void	print_array(int *array, int size)
{
	int i;

	i = 0;
	while (i < size)
	{
		printf("%d ",array[i]);
		i++;
	}
	printf("\n");
}
void	print_square(t_square s)
{
	printf("corner: ");
	print_point(s.corner);
	printf("size: %d\n", s.size);
}

void	print_matrix(int **data, int n)
{
	int i;
	int j;

	for (i = 0; i < n; i++)
	{
		printf("%d: ", i);
		for (j = 0; data[i][j] != INF; j++)
		{
			printf("%d ", data[i][j]);
		}
		printf("\n");
	}
}

void	print_data_map(t_map m)
{
	printf("orizontal :\n");
	print_matrix(m.oriz, m.n);
	printf("vertical :\n");
	print_matrix(m.vert, m.m);
}

void	print_visual_map(t_map m, char obst, char free)
{
	int i;
	int j;
	int k;

	printf("%d %d \n", m.n, m.m);	
	for (i = 0; i < m.n; i++)
	{
		k = 0;
		for (j = 0; j < m.m; j++)
		{
			if (j == m.oriz[i][k])
			{
				printf("%c",obst);	
				k++;
			}
			else
				printf("%c", free);
		}
		printf("\n");
	}
}
