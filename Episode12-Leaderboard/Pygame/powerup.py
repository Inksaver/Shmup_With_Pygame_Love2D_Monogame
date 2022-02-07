import pygame, random
import shared, circle

class Pow():
	def __init__(self, images:dict, center:tuple):
		''' dictionary of 2 images passed, gun and shield '''
		self._key = random.choice(['shield', 'gun'])	# choose one image: 'gun' or 'shield'						# 
		self.image = images[self._key]					# assign to self.image
		self.image.set_colorkey(shared.BLACK)			# set transparency
		self._rect = self.image.get_rect()
		self._rect.center = center
		self._circle = circle.Circle(self._rect.centerx, self._rect.centery, self._rect.width / 2)
		self.speedy = 500
		if shared.debug:
			self.speedy = 250
		self.active = True		

	@property
	def key(self):
		return self._key
	
	@property
	def rect(self):
		return self._rect

	@property
	def circle(self):
		return self._circle		

	def update(self, dt):
		self._rect.y += self.speedy * dt
		self._circle.center = self._rect.center
		# delete if moves off bottom of screen
		if self._rect.top > shared.HEIGHT:
			self.active = False

		return self.active

	def draw(self) -> None:
		shared.screen.blit(self.image, self._rect)
		if shared.debug:
			pygame.draw.circle(shared.screen, shared.RED, self._circle.center, self._circle.radius, 1)	