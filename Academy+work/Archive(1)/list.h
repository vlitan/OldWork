#ifndef LIST_H
#define LIST_H
#include "ft_list.h"
t_list	*ft_list_last(t_list *begin_list);
void	ft_list_push_back(t_list **begin_list, void *data);
void	ft_list_push_front(t_list **begin_list, void *data);
t_list	*ft_list_push_params(int ac, char **av);
int		ft_list_size(t_list *begin_list);
void	ft_putlist(t_list *l, int type, int new);
#endif
