import pygame
import shared

class Bullet():
	def __init__(self, img, x, y) -> None:
		self.image = img
		self.image.set_colorkey(shared.BLACK)		
		self.rect = self.image.get_rect() # filled rectangle as taken from image
		self.rect.bottom = y
		self.rect.centerx = x
		self.speedy = 1000
		if shared.debug:
			self.speedy = 400
		self.active = True
		
	def get_rect(self):
		return self.rect	

	def update(self, dt) -> bool:
		self.rect.y -= round(self.speedy * dt)
		# delete if moves off top of screen
		if self.rect.bottom < 0:
			self.active = False
			
		return self.active
	
	def draw(self) -> None:
		if shared.debug:
			pygame.draw.rect(shared.screen, shared.YELLOW, self.rect, 1)
		else:
			shared.screen.blit(self.image, self.rect)