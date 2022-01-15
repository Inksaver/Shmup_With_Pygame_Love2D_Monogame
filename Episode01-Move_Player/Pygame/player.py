import pygame
import shared

def init(w, h, speed, clr):
	global rect, speedx, colour			# define variables that can be used in all 
	rect = pygame.Rect(0, 0, w, h)		# create a rectangle at 0,0 measuring w x h
	rect.centerx = shared.WIDTH * 0.5	# set the x position 
	rect.bottom = shared.HEIGHT - 10	# set the y position
	speedx = speed 						# high value as delta time is used
	colour = clr						# set colour to GREEN

def update(keystate, dt):
	''' 
	calculate distance to move the rectangle
	Pygame rect is integer only, so using delta time
	requires rounding of calculations to int values
	'''
	if keystate[pygame.K_LEFT] or keystate[pygame.K_a]:
		rect.x -= round(speedx * dt)
	if keystate[pygame.K_RIGHT] or keystate[pygame.K_d]:
		rect.x += round(speedx * dt)
	''' Check if rectangle is out of bounds '''
	if rect.right > shared.WIDTH:
		rect.right = shared.WIDTH
	if rect.left < 0:
		rect.left = 0
		
def draw():
	pygame.draw.rect(shared.screen, colour, rect)