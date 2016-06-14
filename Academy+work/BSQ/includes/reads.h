#ifndef	READS_H
# define READS_H
void	get_next_input(char *argv, char *obst, char *free, char *full);
void	read_input(int fd, char *obst, char *free, char *full);
int		get_items(int fd, char *obst, char *free, char *full);
int		*get_from_line(int *answ, char *line, int size, char obst, char free);
void	copy_data(int *to, int *from, int count);
int		is_valid_char(char in, char obst, char free);
int		ft_atoi(char *str, int *size);
char	*ft_read_to(int fd, char end, int buff_s);
#endif
