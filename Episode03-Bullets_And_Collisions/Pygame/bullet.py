import pygame
import shared

class Bullet():
	def __init__(self, x:int, y:int, w:int, h:int, colour:tuple) -> None:
		self.rect = pygame.Rect(x, y, w, h)		# create a rectangle at 0,0 measuring w x h	
		self.colour = colour					# set colour
		self.rect.bottom = y
		self.rect.centerx = x
		self.speedy = 1000
		if shared.debug:
			self.speedy = 400
		self.active = True
		
	def get_rect(self):
		return self.rect

	def update(self, dt) -> None:
		self.rect.y -= round(self.speedy * dt)
		# delete if moves off top of screen
		if self.rect.bottom < 0:
			self.active = False
			
		return self.active
			
	def draw(self) -> None:
		pygame.draw.rect(shared.screen, self.colour, self.rect)
