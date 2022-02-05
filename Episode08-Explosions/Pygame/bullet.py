import pygame
import shared, circle

class Bullet():
	def __init__(self, img, x, y) -> None:
		self.image = img
		self.image.set_colorkey(shared.BLACK)		
		self.rect = self.image.get_rect() # filled rectangle as taken from image
		self.rect.bottom = y
		self.rect.centerx = x
		self.speedy = 1000
		if shared.debug:
			self.speedy = 500
		self.active = True	
		self.circle = circle.Circle(self.rect.centerx, self.rect.centery, self.rect.width / 2)
		self.colour = shared.RED

	def get_rect(self):
		return self.rect

	def get_circle(self):
		self.circle.x = self.rect.centerx
		self.circle.y = self.rect.y	+ self.circle.radius	
		return self.circle

	def update(self, dt) -> bool:
		self.rect.y -= round(self.speedy * dt)
		self.circle.y = self.rect.y
		# delete if moves off top of screen
		if self.rect.bottom < 0:
			self.active = False

		return self.active

	def draw(self) -> None:
		if shared.debug:
			pygame.draw.circle(shared.screen, self.colour, self.circle.center, self.circle.radius, 1)
			pygame.draw.rect(shared.screen, shared.YELLOW, self.rect, 1)	
		else:
			shared.screen.blit(self.image, self.rect)