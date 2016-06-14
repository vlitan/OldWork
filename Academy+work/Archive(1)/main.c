#include "list.h"
#include <stdio.h>
void ft_list_reverse(t_list **begin_list);
int main(int argc, char **argv)
{
	t_list *l;
	int a = 4;
	int b = 3;
	int c = 2;
	l = ft_list_push_params(argc, argv);
	printf("inainte:");
	ft_putlist(l, 2, 1);
	ft_list_reverse(&l);
	printf("dupa:");
	ft_putlist(l, 2, 1); 
	return (0);
}
