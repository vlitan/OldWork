#ifndef BSQ_H
# define BSQ_H
# define INF 0x7fffffff
typedef struct		s_chars
{
	char			free;
	char			full;
	char			obst;
}					t_chars;

typedef struct		s_point
{
	int				x;
	int				y;
}					t_point;

typedef	struct		s_square
{
	t_point			corner;
	unsigned int	size;
}					t_square;

typedef struct		s_map
{
	int				**oriz;
	int				n;
	int				m;
}					t_map;
#endif
