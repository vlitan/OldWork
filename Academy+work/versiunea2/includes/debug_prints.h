#ifndef DEBUG_PRINTS_H
# define DEBUG_PRINTS_H
# include "BSQ.h"
void	print_array(int *array, int size);
void	print_point(t_point p);
void	print_square(t_square s);
void	print_matrix(int **data, int n, int m);
void	print_data_map(t_map m);
void	print_visual_map(t_map m, char obst, char free);
#endif
