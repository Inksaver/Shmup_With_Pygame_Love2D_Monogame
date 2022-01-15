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
window_title:str           = ""
screen:object          = None
clock:object           = None
audio_present:bool     = True # if run on a PC without audio eg at school
'''                    game variables '''
debug                  = False
gamestates:dict        = {"menu":0, "play":1, "quit":2}
gamestate:int		   = 0