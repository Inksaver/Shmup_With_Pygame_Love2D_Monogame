import pygame
'''                    game constants '''
WIDTH:int              = 800   			# Window width: keep within screen resolution eg max 1366
HEIGHT:int             = 600    		# Window height: keep within screen resolution eg max 768
FPS:int                = 60        		# Frames Per Second: controls the running speed of the game
WHITE:tuple            = (255,255,255)
BLACK:tuple            = (0,0,0)
RED:tuple              = (255, 0, 0)
GREEN:tuple            = (0, 255, 0)
BLUE:tuple             = (0 ,0 , 255)
CYAN:tuple             = (0, 255, 255)
MAGENTA:tuple          = (255, 0 , 255)
YELLOW:tuple           = (255, 255, 0)
DARK_BLUE:tuple		   = (0,0,12)
'''                    pygame objects  '''
window_title:str       = ""
screen:object          = None
clock:object           = None
audio_present:bool     = True # if run on a PC without audio eg at school
'''                    game variables '''
debug                  = False
gamestates:dict        = {"menu":0, "play":1, "leaderboard":2, "quit":3}
gamestate:int		   = 0
font_name:str          = pygame.font.match_font('arial')
score:int			   = 0
play_music             = False

def draw_text(screen:object, text:str, size:int, x:int, y:int, align:str = 'centre', colour:tuple = WHITE) -> None:
	''' function to draw text '''
	#define font
	font = pygame.font.Font(font_name, size)
	#define drawing surface
	text_surface = font.render(text, True, colour) # True = antialias
	#get the rectangle of this surface
	text_rect = text_surface.get_rect()
	#place the text in mid-top location for now
	if align == 'centre' or align == 'center':
		text_rect.midtop = (x, y)
	elif align == 'left':
		text_rect.topleft = (x, y)
	elif align == 'right':
		text_rect.topright = (x, y)		
	#blit the text to chosen surface
	screen.blit(text_surface, text_rect)

def display_box(screen:object, text:str, text_size:int, box_rect:object, fore_color:tuple = WHITE, back_colour:tuple = DARK_BLUE) -> None:
	''' Print a text in a box '''
	font = pygame.font.Font(None, text_size)
	# pygame.draw.rect(surface, colour, rectangle, lineWidth) lineWidth 0 = fill (default) else line width
	pygame.draw.rect(screen,
					(back_colour),
					(box_rect.x,
					box_rect.y,
					box_rect.width,
					box_rect.height + 2),
					0)
	pygame.draw.rect(screen,
					(fore_color),
					(box_rect.x,
					box_rect.y,
					box_rect.width,
					box_rect.height),
					1)
	# render(text, antialias, color, background=None) -> Surface
	if len(text) != 0:
		draw_text(screen, text, text_size, box_rect.x + 2, box_rect.y + 3, 'left')
		#screen.blit(font.render(text, 1, WHITE, box_rect.x + 2, box_rect.y + 5))

